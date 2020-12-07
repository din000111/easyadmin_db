using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public static class DatacenterService
    {
        public static Task<ServicesResponse<List<Datacenter>>> GetDatacentersListAsync (Adapter adapter) {
            return ApiService.GetRequestAsync<List<Datacenter>>(adapter, "datacenters");
        }
    }
}
