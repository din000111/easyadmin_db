using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public class TaskService
    {
        public static async Task<ServicesResponse<BackendTask>> GetTaskState(Adapter adapter, BackendTask task)
        {
            return await ApiService.GetRequestAsync<BackendTask>(adapter, $"task/{task.BackendTaskId}");
        }            
    }
}
