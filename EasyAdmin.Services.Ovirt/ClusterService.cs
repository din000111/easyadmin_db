using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt;
using System.Threading.Tasks;

namespace EasyAdmin.Services.Ovirt
{
    public static class ClusterService
    {

        public static async Task<ServicesResponse> GetClusters(Adapter adapter)
        {
            ServicesResponse servicesResponse = await ApiService.GetRequest(adapter, typeof(Clusters), "ovirt-engine/api/clusters");
            servicesResponse.resultObject = (Clusters)servicesResponse.resultObject;
            return servicesResponse;
        }
    }
}
