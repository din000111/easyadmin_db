using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasyAdmin.Services.Ovirt
{
    public static class ApiService
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = delegate { return true; } });
        
        private static object XmlDeserialize(string responseBody, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (TextReader reader = new StringReader(responseBody))
            {
                return serializer.Deserialize(reader);
            }
        }
        private static T XmlDeserialize<T>(string responseBody)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(responseBody))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        private static string XmlSerialize(object objectIn, Type type)
        {
            string xmlString;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objectIn);
                xmlString = textWriter.ToString();
            }
            return xmlString;
        }
        private static string XmlSerialize<T>(T objectIn)
        {
            string xmlString;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objectIn);
                xmlString = textWriter.ToString();
            }
            return xmlString;
        }
        private static void SetHeaders(Adapter adapter)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                                            "Basic", Convert.ToBase64String(
                                                                Encoding.ASCII.GetBytes(
                                                                   $"{adapter.Credentials.Username}:{adapter.Credentials.Password}")));
        }
        public static async Task<ServicesResponse> GetRequest(Adapter adapter, Type type, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);

            HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = XmlDeserialize(responseBody, type);
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {                    
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse> GetRequest<T>(Adapter adapter, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);

            HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = XmlDeserialize(responseBody, typeof(T));
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse<T>> GetRequest2<T>(Adapter adapter, string endpoint)
        {
            var servicesResponse = new ServicesResponse<T>();
            SetHeaders(adapter);

            HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = (T)XmlDeserialize(responseBody, typeof(T));
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse> GetRequest(Adapter adapter, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);
            try
            {
                HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));
                if (response.IsSuccessStatusCode)
                {
                    servicesResponse.isSuccess = true;
                }
                else
                {
                    servicesResponse.errorCode = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var fault = XmlDeserialize<Fault>(errorMessage);
                        servicesResponse.errorMessage = fault.Detail;
                    }
                    catch (Exception)
                    {
                        servicesResponse.errorMessage = errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = ex.Message;
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse> GetFile(Adapter adapter, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);
            try
            {
                HttpResponseMessage response = await Client.GetAsync(new Uri(adapter.uri, endpoint));
                if (response.IsSuccessStatusCode)
                {
                    servicesResponse.isSuccess = true;
                    servicesResponse.resultObject = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    servicesResponse.errorCode = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var fault = XmlDeserialize<Fault>(errorMessage);
                        servicesResponse.errorMessage = fault.Detail;
                    }
                    catch (Exception)
                    {
                        servicesResponse.errorMessage = errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = ex.Message;
            }
            return servicesResponse;
        }

        public static async Task<ServicesResponse> PostRequest<T>(Adapter adapter, T objectIn, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);

            string xmlString = XmlSerialize<T>(objectIn);
            HttpContent httpContent = new StringContent(xmlString, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await Client.PostAsync(new Uri(adapter.uri, endpoint), httpContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = XmlDeserialize(responseBody, typeof(T));
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse<T>> PostRequest2<T>(Adapter adapter, T objectIn, string endpoint)
        {
            var servicesResponse = new ServicesResponse<T>();
            SetHeaders(adapter);

            string xmlString = XmlSerialize<T>(objectIn);
            HttpContent httpContent = new StringContent(xmlString, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await Client.PostAsync(new Uri(adapter.uri, endpoint), httpContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = (T)XmlDeserialize(responseBody, typeof(T));
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
        public static async Task<ServicesResponse> DeleteRequest(Adapter adapter, Type type, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);

            HttpResponseMessage response = await Client.DeleteAsync(new Uri(adapter.uri, endpoint));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = XmlDeserialize(responseBody, type);
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }

        public static async Task<ServicesResponse> PutRequest<objIn, objOut>(Adapter adapter, object objectIn, string endpoint)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            SetHeaders(adapter);
            
            string xmlString = XmlSerialize(objectIn, typeof(objIn));
            HttpContent httpContent = new StringContent(xmlString, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await Client.PutAsync(new Uri(adapter.uri, endpoint), httpContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                servicesResponse.resultObject = XmlDeserialize(responseBody, typeof(objOut));
                servicesResponse.isSuccess = true;
            }
            else
            {
                servicesResponse.errorCode = (int)response.StatusCode;
                var errorMessage = await response.Content.ReadAsStringAsync();
                try
                {
                    var fault = XmlDeserialize<Fault>(errorMessage);
                    servicesResponse.errorMessage = fault.Detail;
                }
                catch (Exception)
                {
                    servicesResponse.errorMessage = errorMessage;
                }
            }
            return servicesResponse;
        }
    }
}
