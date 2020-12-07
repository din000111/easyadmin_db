using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt.StorageDomains;
using System.Threading.Tasks;

namespace EasyAdmin.Services.Ovirt
{
    public static class StorageDomainsService
    {
        public static async Task<ServicesResponse> GetStorageDomains(Adapter adapter)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }

            servicesResponse = await ApiService.GetRequest(adapter, typeof(StorageDomains), "ovirt-engine/api/storagedomains");
            servicesResponse.resultObject = (StorageDomains)servicesResponse.resultObject;

            return servicesResponse;
        }
    }
}
