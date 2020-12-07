using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService
    {
        public async Task<ApiResponse<Adapter>> CreateAdapterAsync(Adapter adapter)
        {
            adapter.ProviderId = (int)adapter.Provider.Id;
            adapter.Provider = null;
            return await InvokeApiPostRequestAsync<Adapter, Adapter>("api/adapters/new", adapter);
        }

        public async Task<ApiResponse<bool>> DeleteAdapterAsync(Adapter adapter)
        {
            return await InvokeApiDeleteRequestAsync<bool>($"api/adapters/{adapter.Id}");
        }

        public Task<ApiResponse<Adapter>> GetAdapterAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<Adapter>>> GetAdaptersListAsync()
        {
            var api = await InvokeApiGetRequestAsync<List<Adapter>>("api/adapters");
            return api;
        }



        public async Task<ApiResponse<bool>> TestAdapterAsync(Adapter adapter)
        {
            return await InvokeApiPostRequestAsync<Adapter, bool>("api/adapters/test", adapter);
        }

        public async Task<ApiResponse<bool>> TurnOffAdapterAsync(Adapter adapter)
        {
            return await InvokeApiGetRequestAsync<bool>($"api/adapters/turnoff/{adapter.Id}");
        }

        public async Task<ApiResponse<Adapter>> UpdateAdapterAsync(Adapter adapter)
        {
            var api = await InvokeApiPutRequestAsync<Adapter>($"api/adapters/{adapter.Id}", adapter);
            return api;
        }
    }
}
