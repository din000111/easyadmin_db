using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyAdmin.Shared.Common;

namespace EasyAdmin.Shared.Common
{
    public class Adapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CredetialsId { get; set; }
        public virtual Credentials Credentials { get; set; }

        public string Address { get; set; }
        public Uri uri => new Uri(Address);
        
        public int ProviderId { get; set; }
        public virtual Provider Provider { get; set; }
        public bool IsOK { get; set; }

        public string Status { get; set; }
    }
}
