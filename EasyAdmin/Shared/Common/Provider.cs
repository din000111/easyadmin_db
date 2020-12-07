using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyAdmin.Shared.Common
{
    public class Provider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ProviderType ProviderType { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public virtual IEnumerable<Adapter> Adapters { get; set; }
    }
    public enum ProviderType { Ovirt=1, VMware=2}
}
