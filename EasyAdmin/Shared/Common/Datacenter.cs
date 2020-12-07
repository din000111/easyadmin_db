using System.Collections.Generic;
using EasyAdmin.Shared.Ovirt.Datacenter;

namespace EasyAdmin.Shared.Common
{
    public class Datacenter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Adapter Adapter { get; set; }
        public virtual List<Cluster> Clusters { get; set; }
        public virtual Cost Cost { get; set; }
        public static implicit operator Datacenter(DataCenter ovirtDatacenter)
        {
            Datacenter datacenter = new Datacenter
            {
                Id = ovirtDatacenter.Id,
                Name = ovirtDatacenter.Name,
                Description = ovirtDatacenter.Description
            };

            return datacenter;
        }
    }
}
