namespace EasyAdmin.Shared.Common
{
    public class Cluster
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual Datacenter Datacenter { get; set; }
        public string Id { get; set; }
        public virtual Audit Audit { get; set; }


        public static implicit operator Cluster(Ovirt.Cluster ovirtCluster)
        {
            var cluster = new Cluster
            {
                Name = ovirtCluster?.Name,
                ShortName = ovirtCluster?.Name,
                Id = ovirtCluster.Id,
                Datacenter = new Datacenter
                {
                    Id = ovirtCluster?.DataCenter?.Id
                }
            };
            return cluster;
        }
    }    
}
