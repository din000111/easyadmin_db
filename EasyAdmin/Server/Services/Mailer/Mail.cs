using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Server.Services.Mailer
{
    public class Mail
    {
        public string Address { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? AuthorizationRequired { get; set; }
    }
}
