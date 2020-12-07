﻿namespace EasyAdmin.Server.Services.Authentication
{
    public class Ldap
    {

        public string Url { get; set; }
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string BindDn { get; set; }

        public string BindCredentials { get; set; }

        public string SearchBase { get; set; }

        public string SearchFilter { get; set; }

        public string AdminCn { get; set; }
        public string SearchGroupBase { get; set; }
    }
}
