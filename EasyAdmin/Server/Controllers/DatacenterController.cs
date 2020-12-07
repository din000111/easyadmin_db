using EasyAdmin.Services.Ovirt;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OvirtDatacenter = EasyAdmin.Shared.Ovirt.Datacenter;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatacenterController : Controller
    {
        private readonly EasyAdminContext _context;
        public DatacenterController(EasyAdminContext context)
        {
            _context = context;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [Route("all")]
        public async Task<ActionResult<List<Datacenter>>> GetDatacenters(Adapter adapter)
        {
            if (adapter?.Provider == null)
            {
                return BadRequest();
            }
            adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials).Include(p => p.Provider).SingleOrDefaultAsync(a => a.Id == adapter.Id);

            List<Datacenter> datacenters = new List<Datacenter>();
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    ServicesResponse servicesResponse = await DatacenterService.GetDatacenters(adapter);
                    if (servicesResponse.isSuccess)
                    {
                        OvirtDatacenter.DataCenters ovirtDataCenters = (OvirtDatacenter.DataCenters)servicesResponse.resultObject;
                        datacenters.AddRange(ovirtDataCenters.DataCenter.ConvertAll(x => (Datacenter)x));
                    }
                    break;
            }
            return datacenters;
        }
        [HttpPost]
        public async IAsyncEnumerable<Datacenter> GetDatacentersSafe(Adapter adapter)
        {
            if (adapter?.Provider == null)
            {
                yield break;
            }
            adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials).Include(p => p.Provider).SingleOrDefaultAsync(a => a.Id == adapter.Id);
            switch (adapter.Provider.ProviderType)
            {
                case ProviderType.Ovirt:
                    ServicesResponse servicesResponse = await DatacenterService.GetDatacenters(adapter);
                    if (servicesResponse.isSuccess)
                    {
                        OvirtDatacenter.DataCenters ovirtDataCenters = (OvirtDatacenter.DataCenters)servicesResponse.resultObject;
                        foreach (var dc in ovirtDataCenters.DataCenter)
                        {
                            yield return dc;
                        }
                    }
                    break;
                case ProviderType.VMware:
                    var response = await EasyAdmin.Services.VMware.DatacenterService.GetDatacentersListAsync(adapter);
                    if (response.isSuccess)
                    {
                        foreach (var dc in response.resultObject)
                        {
                            yield return dc;
                        }
                    }
                    break;
            }
        }
    }
}