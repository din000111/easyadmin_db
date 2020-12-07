using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt.Nic;
using System.Threading.Tasks;
using OvirtNetwork = EasyAdmin.Shared.Ovirt.Network;
using OvirtVNics = EasyAdmin.Shared.Ovirt.VNicProfile;


namespace EasyAdmin.Services.Ovirt
{
    public static class NetworkService
    {
        public static async Task<ServicesResponse> GetNetworks(string clusterId, Adapter adapter)
        {
            ServicesResponse servicesResponse = await ApiService.GetRequest(adapter, typeof(OvirtNetwork.Networks), $"ovirt-engine/api/clusters/{clusterId}/networks");
            return servicesResponse;
        }
        public static async Task<ServicesResponse> CreateNic(string vMId, string networkId, Adapter adapter)
        {
            ServicesResponse vNicProfilesResponse = await ApiService.GetRequest(adapter, typeof(OvirtVNics.VnicProfiles), $"ovirt-engine/api/networks/{networkId}/vnicprofiles");
            if (!vNicProfilesResponse.isSuccess || vNicProfilesResponse.resultObject == null)
            {
                return vNicProfilesResponse;
            }
            OvirtVNics.VnicProfiles vNics = (OvirtVNics.VnicProfiles)vNicProfilesResponse.resultObject;
            string vNicProfileId = vNics.VnicProfile[0].Id;
            Nic newNic = new Nic
            {
                Name = "vmnic",
                Interface = "virtio",
                Vnic_profile = new Vnic_profile
                {
                    Id = vNicProfileId
                }
            };
            ServicesResponse vNicResponse = await ApiService.PostRequest<Nic>(adapter, newNic, $"ovirt-engine/api/vms/{vMId}/nics");
            vNicResponse.resultObject = (Nic)vNicResponse.resultObject;
            return vNicResponse;
        }
    }
}
