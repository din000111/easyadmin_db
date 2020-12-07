using Microsoft.AspNetCore.Mvc;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        //static readonly HttpClient client = new HttpClient();

        //[HttpGet]
        //[Route("all")]
        //public async Task<List<Host>> Get()
        //{
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        //                                                    "Basic", Convert.ToBase64String(
        //                                                        System.Text.ASCIIEncoding.ASCII.GetBytes(
        //                                                           $"{OvirtLogin}:{OvirtPassword}")));
        //    HttpResponseMessage response = await client.GetAsync("https://engine42.hostco.ru/ovirt-engine/api/hosts");
        //    response.EnsureSuccessStatusCode();
        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    var o = JObject.Parse(responseBody);
        //    List<Host> hosts = o["host"].ToObject<List<Host>>();
        //    return hosts;
        //}
    }
}