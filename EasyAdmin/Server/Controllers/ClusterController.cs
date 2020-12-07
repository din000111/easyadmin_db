using EasyAdmin.Services.Ovirt;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OvirtClusters = EasyAdmin.Shared.Ovirt.Clusters;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClusterController : Controller
    {
        private readonly EasyAdminContext _context;
        public ClusterController(EasyAdminContext context)
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

        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<Cluster>>> Get(Datacenter datacenter)
        //{
        //    if (datacenter?.Adapter == null)
        //    {
        //        return BadRequest();
        //    }
        //    List<Cluster> clusters = new List<Cluster>();

        //    Adapter adapter = await _context.Adapters.Where(x => x.IsOK).Include(x => x.Credentials).Include(x => x.Provider).SingleOrDefaultAsync(y => y.Id == datacenter.Adapter.Id);

        //    if (adapter?.Provider == null)
        //    {
        //        return BadRequest();
        //    }
        //    switch (adapter.Provider.Name.ToLower())
        //    {
        //        case "ovirt":
        //            ServicesResponse servicesResponse = await ClusterService.GetClusters(adapter);
        //            if (servicesResponse.isSuccess)
        //            {
        //                OvirtClusters ovirtClusters = (OvirtClusters)servicesResponse.resultObject;
        //                clusters.AddRange(ovirtClusters.Cluster.ConvertAll(x => (Cluster)x));
        //            }
        //            break;
        //    }
        //    return Ok(clusters.Where(c => c.Datacenter.Id == datacenter.Id));
        //}
        [HttpPost]
        public async IAsyncEnumerable<Cluster> Get(Datacenter datacenter)
        {
            if (datacenter?.Adapter == null)
            {
                yield return null;
                yield break;
            }
            List<Cluster> clusters = new List<Cluster>();

            Adapter adapter = await _context.Adapters
                .Where(x => x.IsOK)
                .Include(x => x.Credentials)
                .Include(x => x.Provider)
                .SingleOrDefaultAsync(y => y.Id == datacenter.Adapter.Id);

            if (adapter?.Provider == null)
            {
                yield return null;
                yield break;
            }
            switch (adapter.Provider.ProviderType)
            {
                case ProviderType.Ovirt:
                    ServicesResponse servicesResponse = await ClusterService.GetClusters(adapter);
                    if (servicesResponse.isSuccess)
                    {
                        OvirtClusters ovirtClusters = (OvirtClusters)servicesResponse.resultObject;
                        foreach (var cluster in ovirtClusters.Cluster)
                        {
                            if (cluster.DataCenter.Id == datacenter.Id)
                            {
                                yield return cluster;
                            }                            
                        }
                    }
                    break;
                case ProviderType.VMware:
                    var response = await EasyAdmin.Services.VMware.ClusterService.GetClustersListAsync(adapter, datacenter);
                    if (response.isSuccess)
                    {
                        foreach (var cluster in response.resultObject)
                        {
                            yield return cluster;
                        }
                    }
                    break;
            }            
        }
    }
}