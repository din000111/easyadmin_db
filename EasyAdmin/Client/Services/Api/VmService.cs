using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService
    {
        public Task<ApiResponse<List<Vm>>> GetVmListAsync()
        {
            

            throw new NotImplementedException();
        }

        public Task<ApiResponse<Vm>> CreateVmAsync(Vm vm)
        {
            return InvokeApiPostRequestAsync<Vm, Vm>("api/vm", vm);
        }

        public Task<ApiResponse<Vm>> UpdateVmAsync(Vm vm)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse<Vm>> ProlongVmAsync(Vm vm)
        {
            // var response = await Http.PostJsonAsync<BackResponse>("api/vm/prolong", selectedVM);
            return InvokeApiPostRequestAsync<Vm, Vm>("api/vm/prolong", vm);
            throw new NotImplementedException();
        }
    }
}
