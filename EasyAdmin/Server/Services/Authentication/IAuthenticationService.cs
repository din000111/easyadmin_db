using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;

namespace EasyAdmin.Server.Services.Authentication
{
    public interface IAuthenticationService
    {
        User Login(string username, string password);
        ClaimsPrincipal ValidateToken(string jwtToken);
        string CreateToken(List<Claim> userClaims);
        List<User> GetUsersList(List<User> responsibles);
        Task<List<User>> GetUsersAsync(List<string> sams);
        void Dispose();
        List<User> GetUsersList();
        IEnumerable<User> GetUsers(string searchFilter);
    }
}
