using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public static class ClusterService
    {
        public static Task<ServicesResponse<List<Cluster>>> GetClustersListAsync (Adapter adapter, Datacenter datacenter) {
            return ApiService.GetRequestAsync<List<Cluster>>(adapter, $"clusters/{datacenter.Id}");
        }
    }
}
