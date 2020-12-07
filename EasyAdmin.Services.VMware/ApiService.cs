using EasyAdmin.Shared.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;

namespace EasyAdmin.Services.VMware
{
    public static class ApiService
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = delegate { return true; } });

        public static async Task<ServicesResponse<T>> GetRequestAsync<T> (Adapter adapter, string endpoint)
        {
            var servicesResponse = new ServicesResponse<T>();
            try
            {
                HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    servicesResponse.isSuccess = true;
                    servicesResponse.resultObject = JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    servicesResponse.errorCode = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    servicesResponse.errorMessage = errorMessage;
                    //try
                    //{
                    //    var fault = XmlDeserialize<Fault>(errorMessage);
                    //    servicesResponse.errorMessage = fault.Detail;
                    //}
                    //catch (Exception)
                    //{
                    //    servicesResponse.errorMessage = errorMessage;
                    //}
                }
            }
            catch (Exception ex)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = ex.Message;
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse<T2>> PostRequestAsync<T1,T2>(Adapter adapter, T1 t, string endpoint)
        {
            var servicesResponse = new ServicesResponse<T2>();
            try
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(t));
                HttpResponseMessage response = await Client.PostAsync(new Uri(adapter.uri, endpoint), httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    servicesResponse.isSuccess = true;
                    servicesResponse.resultObject = JsonConvert.DeserializeObject<T2>(result);
                }
                else
                {
                    servicesResponse.errorCode = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            catch (Exception ex)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = ex.Message;
            }
            return servicesResponse;
        }
    }
}
