using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService
    {
        public async Task<ApiResponse<List<Provider>>> GetProvidersListAsync()
        {
            return await InvokeApiGetRequestAsync<List<Provider>>("api/providers");
        }
    }
}
