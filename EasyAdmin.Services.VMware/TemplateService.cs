using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public static class TemplateService
    {
        public static async Task<ServicesResponse<List<Template>>> GetTemplatesListAsync(Adapter adapter, Cluster cluster)
        {
            var request = await ApiService.GetRequestAsync<List<Template>>(adapter, $"vms/templates/{cluster.Datacenter.Id}/{cluster.Id}");
            if (request.isSuccess)
            {
                request.resultObject.Add(new Template
                {
                    Name = "Blank",
                    Description = "Новая ВМ"
                });
            }
            return request;
        }
    }
}
