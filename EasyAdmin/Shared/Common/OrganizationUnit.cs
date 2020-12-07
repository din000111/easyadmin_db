using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasyAdmin.Shared.Common
{
    public class OrganizationUnit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DistinguishedName { get; set; }
        public string DisplayName { get; set; }
        public string PoolShortName { get; set; }
        public string AdminsGroupCN { get; set; }
        [NotMapped]
        public string AdminGroupDisplayName { get; set; }
        public int? ParentId { get; set; }
        public virtual OrganizationUnit Parent { get; set; }
        public virtual ICollection<OrganizationUnit> Child { get; }

    }
}
