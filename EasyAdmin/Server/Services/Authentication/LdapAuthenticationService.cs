using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
//TODO временно
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Controls;

namespace EasyAdmin.Server.Services.Authentication
{
    public class LdapAuthenticationService : IAuthenticationService, IDisposable
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SamAccountNameAttribute = "sAMAccountName";

        private const int JwtExpiryInDays = 1;
        private const string JwtIssuer = "http://localhost:46029/";
        private const string JwtAudience = "http://localhost:46030/";
        private readonly string _secretKey;

        private readonly Ldap _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<Ldap> config, IOptions<Secret> secretKey)
        {
            _config = config.Value;
            _secretKey = secretKey.Value.SecretKey;            
            _connection = new LdapConnection
            {
                SecureSocketLayer = _config.UseSsl,
                Constraints = new LdapSearchConstraints { ReferralFollowing = true }
            };
        }

        public User Login(string username, string password)
        {
            //TODO временно
            _connection.UserDefinedServerCertValidationDelegate += new Novell.Directory.Ldap.RemoteCertificateValidationCallback(MySSLHandler);

            _connection.Connect(_config.Url, _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);

            // string searchFilter = $"(&(objectClass=User)(extensionAttribute1=*)(sAMAccountName={username}))";// string.Format(_config.SearchFilter, username);
            string searchFilter = string.Format(_config.SearchFilter, $"(sAMAccountName={username})");
            LdapSearchResults result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SamAccountNameAttribute, "sn", "givenName", "distinguishedName", "cn" },
                false
            );
            if (!result.HasMore())
            {
                return null;
            }
            LdapEntry user = result.Next();
            if (user != null)
            {
                _connection.Bind(user.DN, password);
                if (_connection.Bound)
                {                        
                    return new User
                    {
                        DisplayName = $"{user.getAttribute("sn")?.StringValue ?? "noSN"} {user.getAttribute("givenName")?.StringValue ?? "noGivenName"}",
                        Sam = user.getAttribute(SamAccountNameAttribute)?.StringValue ?? "noSam",
                        IsAdmin = user.getAttribute(MemberOfAttribute)?.StringValueArray.Contains(_config.AdminCn) ?? false,
                        DistinguishedName = user.getAttribute("distinguishedName")?.StringValue ?? "noDn",
                        Subordinates = GetSubordinates(user.getAttribute("distinguishedName")?.StringValue)
                    };
                }
            }            
            _connection.Disconnect();
            return null;
        }
        public List<User> GetUsersList()
        {
            //TODO временно
            _connection.UserDefinedServerCertValidationDelegate += new Novell.Directory.Ldap.RemoteCertificateValidationCallback(MySSLHandler);
            
            _connection.Connect(_config.Url, _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);            

            if (_connection.Bound)
            {
                var users = GetUsers(string.Format(_config.SearchFilter, "(sAMAccountName=*)")).ToList();
                users.Sort((x, y) => string.CompareOrdinal(x.DisplayName, y.DisplayName));
                return users;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<User>> GetUsersAsync(List<string> usersSam)
        {
            List<User> users;
            //todo временно
            _connection.UserDefinedServerCertValidationDelegate += new Novell.Directory.Ldap.RemoteCertificateValidationCallback(MySSLHandler);
            _connection.Connect(_config.Url, _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);
            
            string userFilter = "(|";
            foreach (var username in usersSam)
            {
                userFilter += $"(sAMAccountName={username})";
            }
            userFilter += ")";
           
            string searchFilter = string.Format(_config.SearchFilter, userFilter);
            Console.WriteLine(searchFilter);
            if (_connection.Bound)
            {
                users = GetUsers(searchFilter).ToList();
                users.Sort((x, y) => string.CompareOrdinal(x.DisplayName, y.DisplayName));
                return users;
            }
            else
            {
                return null;
            }            
        }
        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,

                ValidAudience = JwtAudience.ToLower(),
                ValidIssuer = JwtIssuer.ToLower(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
            return principal;
        }

        public string CreateToken(List<Claim> userClaims)
        {
            //var principal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, GetType().Name));
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(Convert.ToInt32(JwtExpiryInDays));

            //var token = new JwtSecurityToken(
            //    _JwtIssuer,
            //    _JwtAudience,
            //    userClaims,
            //    expires: expiry,
            //    signingCredentials: creds
            //);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(userClaims, "jwt");


            SecurityToken token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Audience = JwtAudience,
                Issuer = JwtIssuer,
                SigningCredentials = creds,
                Expires = expiry,
                Subject = identity
            });

            return tokenHandler.WriteToken(token);

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IEnumerable<string> GetSubordinates(string managerCn)
        {
            var subs = new List<string>();
            if (managerCn == null)
            {
                return subs;
            }
            string searchFilter = $"(&(objectClass=User)(manager={managerCn}))";// string.Format(_config.SearchFilter, username);
            LdapSearchResults result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { SamAccountNameAttribute },
                false
            );
            foreach (var user in result)
            {
                subs.Add(user.getAttribute(SamAccountNameAttribute).StringValue);
            }
            return subs;
        }
        private List<User> LdapSearch(string searchFilter)
        {
            LdapSearchResults result = _connection.Search(
                    _config.SearchBase,
                    LdapConnection.SCOPE_SUB,
                    searchFilter,
                    new[] { MemberOfAttribute, DisplayNameAttribute, SamAccountNameAttribute, "sn", "givenName", "mail" },
                    false
                );
            List<User> users = new List<User>();
            foreach (LdapEntry ldapUser in result)
            {                
                users.Add(new User
                {
                    DisplayName = $"{ldapUser.getAttribute("sn")?.StringValue ?? "noSn"} {ldapUser.getAttribute("givenName")?.StringValue ?? "noGivenName"}",
                    Sam = ldapUser.getAttribute(SamAccountNameAttribute)?.StringValue ?? "noSam",
                    IsAdmin = ldapUser.getAttribute(MemberOfAttribute)?.StringValueArray.Contains(_config.AdminCn) ?? false,
                    Subordinates = null,
                    Email = ldapUser.getAttribute("mail")?.StringValue ?? "noMail"
                });                
            }
            return users;
        }
        public IEnumerable<User> GetUsers(string searchFilter) {
            _connection.Connect(_config.Url, _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);

            int startIndex = 1;
            int contentCount = 0;
            int afterIndex = 1000;
            int count = 0;
            do
            {
                LdapVirtualListControl ctrl = new LdapVirtualListControl(startIndex, 0, afterIndex, contentCount);
                LdapSortKey[] keys = new LdapSortKey[1];
                keys[0] = new LdapSortKey("samaccountname");
                LdapSortControl sort = new LdapSortControl(keys, true);

                LdapSearchConstraints constraints = _connection.SearchConstraints;
                constraints.setControls(new LdapControl[] { ctrl, sort});

                _connection.Constraints = constraints;
                LdapSearchResults lsc = _connection.Search(
                            _config.SearchBase,
                            LdapConnection.SCOPE_SUB,
                            searchFilter,
                            new[] { MemberOfAttribute, DisplayNameAttribute, SamAccountNameAttribute, "sn", "givenName", "mail", "displayName" },
                            false,
                            constraints);
                
                foreach (var item in lsc.ToList())
                {
                    var user = new User
                                {
                                    DisplayName = $"{item.getAttribute("displayName")?.StringValue ?? "noSn"}",
                                    Sam = item.getAttribute(SamAccountNameAttribute)?.StringValue ?? "noSam",
                                    IsAdmin = item.getAttribute(MemberOfAttribute)?.StringValueArray.Contains(_config.AdminCn) ?? false,
                                    Subordinates = null,
                                    Email = item.getAttribute("mail")?.StringValue ?? "noMail"
                                };
                    yield return user;
                }               

                LdapControl[] controls = lsc.ResponseControls;
                if (controls == null)
                {
                    Console.Out.WriteLine("No controls returned");
                }
                else
                {
                    foreach (LdapControl control in controls)
                    {
                        if (control.ID == "2.16.840.1.113730.3.4.10")
                        {
                            LdapVirtualListResponse response = new LdapVirtualListResponse(control.ID, control.Critical, control.getValue());
                            startIndex += afterIndex + 1;
                            contentCount = response.ContentCount;
                            count += afterIndex;
                        }
                    }
                }
                // Console.WriteLine(i);

            } while (count <= contentCount);
            yield break;
        }
        private static LdapControl GetListControl(int page, int pageSize) {
            var index = page * pageSize + 1;
            var before = 0;
            var after = pageSize - 1;
            var count = 0;
            return new LdapVirtualListControl(index, before, after, count);
        }

        private static int? GetTotalCount(LdapSearchResults results) {
            if (results.ResponseControls != null) {
                var r = (from c in results.ResponseControls
                        let d = c as LdapVirtualListResponse
                        where (d != null)
                        select (LdapVirtualListResponse) c).SingleOrDefault();
                if (r != null) {
                    return r.ContentCount;
                }
            }
            return null;
        }   

        public void Dispose()
        {            
            _connection.Dispose();                    
        }

        public List<User> GetUsersList(List<User> responsibles)
        {
            if (responsibles == null)
            {
                return null;
            }

            _connection.Connect(_config.Url, _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);
            
            string userFilter = "(|";
            foreach (var username in responsibles)
            {
                userFilter += $"(sAMAccountName={username.Sam})";
            }
            userFilter += ")";

            string searchFilter = string.Format(_config.SearchFilter, userFilter);
            Console.WriteLine(searchFilter);
            if (_connection.Bound)
            {
                var users = GetUsers(searchFilter).ToList();
                users.Sort((x, y) => string.CompareOrdinal(x.DisplayName, y.DisplayName));
                return users;
            }
            else
            {
                return null;
            }
        }
        bool MySSLHandler(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            //Import the details of the certificate from the server.
            X509Certificate x509 = null;
            X509CertificateCollection coll = new X509CertificateCollection();
            byte[] data = certificate.GetRawCertData();
            x509 = new X509Certificate(data);
            store.Certificates.Add(x509);
            return true;
        }
    }
}
