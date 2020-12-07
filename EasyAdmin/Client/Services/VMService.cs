using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;

namespace EasyAdmin.Client.Services
{
    public interface IVMService
    {
        Task<List<Vm>> GetVMsAsync();
        Vm GetVM();
        ServicesResponse ShutdownVM();
        ServicesResponse StartVM();

    }
    public class VMService: IVMService
    {
        static readonly HttpClient client = new HttpClient();

        public Vm GetVM()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Vm>> GetVMsAsync()
        {
            var response  = await client.GetAsync("api/vm/all");
            List<Vm> list = new List<Vm>();
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception();
            }
            
            return list;
        }

        public ServicesResponse ShutdownVM()
        {
            throw new NotImplementedException();
        }

        public ServicesResponse StartVM()
        {
            throw new NotImplementedException();
        }
    }
}
