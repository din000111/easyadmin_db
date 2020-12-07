using EasyAdmin.Services.Ovirt;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ovirt = EasyAdmin.Shared.Ovirt.Network;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NetworkController : Controller
    {
        private readonly EasyAdminContext _context;
        public NetworkController(EasyAdminContext context)
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
        public async Task<ActionResult<IEnumerable<Network>>> Get(Cluster cluster)
        {
            if (cluster == null)
            {
                return BadRequest();
            }
            Adapter adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials).Include(p => p.Provider).SingleOrDefaultAsync(a => a.Id == cluster.Datacenter.Adapter.Id);

            if (adapter?.Provider == null)
            {
                return BadRequest();
            }
            List<Network> networks = new List<Network>();
            switch (adapter.Provider.ProviderType)
            {
                case ProviderType.Ovirt:
                    ServicesResponse servicesResponse = await NetworkService.GetNetworks(cluster.Id, adapter);
                    if (servicesResponse.isSuccess)
                    {
                        Ovirt.Networks ovirtNetworks = (Ovirt.Networks)servicesResponse.resultObject;
                        networks.AddRange(ovirtNetworks.Network.ConvertAll(x => (Network)x));
                    }
                    break;
                case ProviderType.VMware:
                    var response = await EasyAdmin.Services.VMware.NetworkService.GetNetworksListAsync(adapter, cluster);
                    if (response.isSuccess)
                    {
                        return response.resultObject;
                    }
                    break;
            }
            return networks;
        }
    }
}