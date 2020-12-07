using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt.Datacenter;
using System.Threading.Tasks;

namespace EasyAdmin.Services.Ovirt
{
    public static class DatacenterService
    {
        public static async Task<ServicesResponse> GetDatacenters(Adapter adapter)
        {
            ServicesResponse servicesResponse = await ApiService.GetRequest(adapter, typeof(DataCenters), "ovirt-engine/api/datacenters");
            servicesResponse.resultObject = (DataCenters)servicesResponse.resultObject;
            return servicesResponse;
        }
    }
}
