using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EasyAdmin.Shared.Common
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Sam { get; set; }
        public bool IsAdmin { get; set; }
        public string DistinguishedName { get; set; }
        [NotMapped]
        public IEnumerable<string> Subordinates { get; set; }
    }
}