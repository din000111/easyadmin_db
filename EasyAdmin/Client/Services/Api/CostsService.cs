using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService
    {
        public async Task<ApiResponse<List<Cost>>> GetCostsListAsync()
        {
            return await InvokeApiGetRequestAsync<List<Cost>>("api/costs");
        }

        public async Task<ApiResponse<Cost>> CreateCostAsync(Cost cost)
        {
            return await InvokeApiPostRequestAsync<Cost, Cost>("api/costs", cost);
        }

        public async Task<ApiResponse<Cost>> UpdateCostAsync(Cost cost)
        {
            return await InvokeApiPutRequestAsync($"api/costs/{cost.Id}", cost);
        }

        public async Task<ApiResponse<bool>> DeleteCostAsync(Cost cost)
        {
            // await Http.DeleteAsync($"api/adapters/{selectedCost.Id}");
            return await InvokeApiDeleteRequestAsync<bool>($"api/adapters/{cost.Id}");
        }
    }
}
