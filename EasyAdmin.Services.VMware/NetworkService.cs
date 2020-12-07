using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public static class NetworkService
    {
        public static Task<ServicesResponse<List<Network>>> GetNetworksListAsync(Adapter adapter, Cluster cluster)
        {
            return ApiService.GetRequestAsync<List<Network>>(adapter, $"networks/{cluster.Datacenter.Id}/{cluster.Id}");
        }
    }
}
