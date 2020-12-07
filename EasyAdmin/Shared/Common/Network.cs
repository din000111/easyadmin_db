namespace EasyAdmin.Shared.Common
{
    public class Network
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VlanID { get; set; }

        public static implicit operator Network(Ovirt.Network.Network ovirtNetwork)
        {
            Network network = new Network
            {
                Id = ovirtNetwork.Id,
                Name = ovirtNetwork.Name,
                Description = ovirtNetwork.Description,
                VlanID = ovirtNetwork.Vlan?.Id
            };
            return network;
        }
    }
}
