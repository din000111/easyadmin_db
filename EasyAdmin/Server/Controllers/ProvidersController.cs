using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : Controller
    {
        private readonly EasyAdminContext _context;

        public ProvidersController(EasyAdminContext context)
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

        // GET: api/Providers
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Provider>>> GettblProvider()
        {
            return await _context.Providers.ToListAsync();
        }
        [HttpGet]
        public IEnumerable<Provider> GetProvidersSafe()
        {
            var providers = _context.Providers;
            foreach (var provider in providers)
            {
                yield return provider;
            }
        }

        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(int id)
        {
            Provider provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }
    }
}
