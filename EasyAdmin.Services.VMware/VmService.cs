using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;

namespace EasyAdmin.Services.VMware
{
    interface IVm
    {
        Task<ServicesResponse<List<Vm>>> GetVmListAsync(Adapter adapter);
    }
    public static class VmService
    {
        public static async Task<ServicesResponse<List<Shared.Common.Vm>>> GetVmListAsync(Adapter adapter)
        {
            var response = await ApiService.GetRequestAsync<List<Vm>>(adapter, "vms");

            return response;
        }
        public static async Task<ServicesResponse<BackendTask>> RequestVm(Adapter adapter, Vm vm)
        {
            var response = await ApiService.PostRequestAsync<Vm, BackendTask>(adapter, vm, "vms");

            return response;
        }

        public static async Task<ServicesResponse<GraphicsConsole>> GetConsole(Adapter adapter, Vm vm)
        {
            return await ApiService.GetRequestAsync<GraphicsConsole>(adapter, $"vms/{vm.Name}/console");
        }

        //private class Vm
        //{
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //    public string Status { get; set; }
        //    public string MemoryGb { get; set; }
        //    public string Cpu { get; set; }
        //    public string HddSize { get; set; }
        //    public string Admin { get; set; }
        //    public string Owner { get; set; }
        //    public string Manager { get; set; }
        //    public string Project { get; set; }
        //    public string Services { get; set; }
        //    public string Domain { get; set; }
        //    public string AuditDate { get; set; }
        //    public string DeadLine { get; set; }
        //}
    }

    
}
