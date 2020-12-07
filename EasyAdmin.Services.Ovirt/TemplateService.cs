using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt;
using System.Threading.Tasks;

namespace EasyAdmin.Services.Ovirt
{
    public static class TemplateService
    {
        public static async Task<ServicesResponse> GetTemplates(Adapter adapter)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }

            servicesResponse = await ApiService.GetRequest(adapter, typeof(Templates), "ovirt-engine/api/templates");
            servicesResponse.resultObject = (Templates)servicesResponse.resultObject;

            return servicesResponse;
        }
    }
}
