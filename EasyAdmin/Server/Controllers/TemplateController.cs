using EasyAdmin.Services.Ovirt;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OvirtTemplates = EasyAdmin.Shared.Ovirt.Templates;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : Controller
    {
        private readonly EasyAdminContext _context;
        public TemplateController(EasyAdminContext context)
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
        public async Task<ActionResult<List<Template>>> GetTemplates(Cluster cluster)
        {
            if (cluster?.Datacenter?.Adapter == null)
            {
                return BadRequest();
            }
            Adapter adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials).Include(p => p.Provider)
                .SingleOrDefaultAsync(a => a.Id == cluster.Datacenter.Adapter.Id);

            if (adapter?.Provider == null)
            {
                return BadRequest();
            }
            List<Template> templates = new List<Template>();
            switch (adapter.Provider.ProviderType)
            {
                case ProviderType.Ovirt:
                    ServicesResponse servicesResponse = await TemplateService.GetTemplates(adapter);
                    if (servicesResponse.isSuccess)
                    {
                        OvirtTemplates ovirtTemplates = (OvirtTemplates)servicesResponse.resultObject;
                        ovirtTemplates.Template = ovirtTemplates.Template.Where(x => (x.Cluster?.Id == cluster.Id || x.Id == "00000000-0000-0000-0000-000000000000")).ToList();
                        templates.AddRange(ovirtTemplates.Template.ConvertAll(x => (Template)x));
                    }
                    break;
                case ProviderType.VMware:
                    var response = await EasyAdmin.Services.VMware.TemplateService.GetTemplatesListAsync(adapter, cluster);
                    if (response.isSuccess)
                    {
                        return response.resultObject;
                    }
                    break;
            }
            return templates;
        }
    }
}