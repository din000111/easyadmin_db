using EasyAdmin.Client.Pages.Modals;
using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt.Disk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public partial class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /*
         * Реализация методов каждого класса находится в отдельных файлах в Api/*Service
         */
        
        private async Task<ApiResponse<T>> InvokeApiGetRequestAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return await HandleApiResponseAsync<T>(response);
        }
        private async Task<ApiResponse<output>> InvokeApiPostRequestAsync<input,output>(string uri, input obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);
            var response = await _httpClient.PostAsync(uri, new StringContent(serializedObject, Encoding.UTF8, "application/json"));
            return await HandleApiResponseAsync<output>(response);
        }
        private async Task<ApiResponse<T>> InvokeApiPutRequestAsync<T>(string uri, T obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);
            var response = await _httpClient.PutAsync(uri, new StringContent(serializedObject,Encoding.UTF8,"application/json"));
            return await HandleApiResponseAsync<T>(response);
        }
        private async Task<ApiResponse<T>> InvokeApiDeleteRequestAsync<T>(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            return await HandleApiResponseAsync<T>(response);
        }        
        private async Task<ApiResponse<T>> HandleApiResponseAsync<T>(HttpResponseMessage response)
        {
            var api = new ApiResponse<T>
            {
                isSuccess = response.IsSuccessStatusCode
            };
            if (api.isSuccess)
            {
                var content = await response.Content.ReadAsStringAsync();
                api.Result = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                api.errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(api.errorMessage);
            }

            return api;
        }

        public async Task<ApiResponse<List<Datacenter>>> GetDatacentersListAsync(Adapter adapter)
        {
            return await InvokeApiPostRequestAsync<Adapter, List<Datacenter>>("api/datacenter", adapter);
        }

        public async Task<ApiResponse<List<Cluster>>> GetClustersListAsync(Datacenter datacenter)
        {
            return await InvokeApiPostRequestAsync<Datacenter, List<Cluster>>("api/cluster", datacenter);
        }
        
        public Task<ApiResponse<List<User>>> GetUsersListAsync()
        {
            //_users = await Http.GetJsonAsync<List<User>>("api/users/all");
            return InvokeApiGetRequestAsync<List<User>>("api/users");
        }

        public Task<ApiResponse<List<OrganizationUnit>>> GetOUsListAsync()
        {
            //_ous = await Http.GetJsonAsync<List<OrganizationUnit>>();
            return InvokeApiGetRequestAsync<List<OrganizationUnit>>("api/organizationUnits/my");
        }
        public async Task<ApiResponse<OrganizationUnit>> CreateOUAsync(OrganizationUnit organizationUnit)
        {
            // var createdOu = await Http.PostJsonAsync<OrganizationUnit>("api/OrganizationUnits/new", ou);
            if (organizationUnit.Parent != null)
            {
                organizationUnit.ParentId = organizationUnit.Parent.Id;
                organizationUnit.Parent = null;
            }            
            return await InvokeApiPostRequestAsync<OrganizationUnit, OrganizationUnit>("api/OrganizationUnits/new", organizationUnit);
        }

        public Task<ApiResponse<List<Template>>> GetTemplatesListAsync(Cluster cluster)
        {
            return InvokeApiPostRequestAsync<Cluster, List<Template>>("api/template", cluster);
        }
        public Task<ApiResponse<List<Network>>> GetNetworksListAsync(Cluster cluster)
        {
            //_networks = await Http.PostJsonAsync<List<Network>>("api/network", _selectedCluster);
            return InvokeApiPostRequestAsync<Cluster, List<Network>>("api/network", cluster);
        }

        public Task<ApiResponse<string>> GetVmDiskAttachmentsAsync(Vm vm)
        {
            return InvokeApiPostRequestAsync<Vm, string>("api/backup", vm);
        }

        public Task<ApiResponse<List<BackendTask>>> GetBackendTasksListAsync()
        {
            return InvokeApiGetRequestAsync<List<BackendTask>>("api/backendtasks");
        }

        public Task<ApiResponse<bool>> HideBackendTask(BackendTask backendTask)
        {
            throw new NotImplementedException();
        }
    }
}
