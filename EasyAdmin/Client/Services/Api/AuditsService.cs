using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService
    {
        public async Task<ApiResponse<List<Audit>>> GetAuditsListAsync()
        {
            //_audits = await Http.GetJsonAsync<List<Audit>>("api/audits/all");
            return await InvokeApiGetRequestAsync<List<Audit>>("api/audits");
        }

        public async Task<ApiResponse<Audit>> CreateAuditAsync(Audit audit)
        {
            // var newAudit = await Http.PostJsonAsync<Audit>("api/audits/new", audit);
            return await InvokeApiPostRequestAsync<Audit, Audit>("api/audits", audit);
        }

        public async Task<ApiResponse<Audit>> UpdateAuditAsync(Audit audit)
        {
            return await InvokeApiPutRequestAsync($"api/audits/{audit.Id}", audit);
        }

        public async Task<ApiResponse<bool>> DeleteAuditAsync(Audit audit)
        {
            return await InvokeApiDeleteRequestAsync<bool>($"api/audits/{audit.Id}");
        }
    }
}
