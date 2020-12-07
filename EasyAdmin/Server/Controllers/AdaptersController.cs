using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdaptersController : Controller
    {
        private readonly EasyAdminContext _context;

        public AdaptersController(EasyAdminContext context)
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

        // GET: api/Adapters
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Adapter>>> GetAdapters()
        {
            return await _context.Adapters.Include(x => x.Provider).ToListAsync();
        }
        [HttpGet]
        public async IAsyncEnumerable<Adapter> GetAdaptersSafe()
        {
            var adapters = _context.Adapters.Include(x => x.Provider);
            foreach (var adapter in adapters)
            {
                yield return adapter;
            }
        }

        // GET: api/Adapters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Adapter>> GetAdapter(int id)
        {
            Adapter adapter = await _context.Adapters.FindAsync(id);

            if (adapter == null)
            {
                return NotFound();
            }

            return adapter;
        }

        // PUT: api/Adapters/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAdapter(int id, Adapter adapter)
        {
            if (adapter == null || id != adapter.Id)
            {
                return BadRequest();
            }
            if (adapter.Credentials.Id == 0)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Credentials> a = _context.Credentials.Add(adapter.Credentials);

                Credentials oldCredentials = _context.Credentials.Where(x => x.Id == adapter.CredetialsId).SingleOrDefault();
                if (oldCredentials != null)
                {
                    _context.Credentials.Remove(oldCredentials);
                }

                adapter.CredetialsId = a.Entity.Id;
            }


            _context.Entry(adapter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdapterExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Adapters
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("new")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Adapter>> PostAdapter(Adapter adapter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (adapter == null)
            {
                throw new Exception();
            }
            if (_context.Adapters
                .Select(x=> new {x.Name, x.uri})
                .ToList()
                .Any(x=> x.uri == adapter.uri && x.Name == adapter.Name)
                )
            {
                return BadRequest("Адаптер с таким именем или адресом уже существует");
            }
            ServicesResponse servicesResponse = new ServicesResponse();
            switch (adapter.Provider?.Name)
            {
                case "ovirt":
                    servicesResponse = await EasyAdmin.Services.Ovirt.ApiService.GetRequest(adapter, "/ovirt-engine/api");
                    break;
            }
            if (servicesResponse.isSuccess)
            {
                adapter.IsOK = true;
                adapter.Status = "Подключено";
            }
            else
            {
                adapter.IsOK = false;
                adapter.Status = servicesResponse.errorMessage;
            }
            _context.Adapters.Add(adapter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdapter", new { id = adapter.Id }, adapter);
        }

        // DELETE: api/Adapters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdapter(int id)
        {
            Adapter adapter = await _context.Adapters.FindAsync(id);
            if (adapter == null)
            {
                return NotFound("Adapter == null");
            }

            _context.Adapters.Remove(adapter);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        private bool AdapterExists(int id)
        {
            return _context.Adapters.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> CheckAdapter(Adapter adapter)
        {
            adapter = _context.Adapters.Include(x => x.Credentials).Include(x => x.Provider).SingleOrDefault(x => x.Id == adapter.Id);
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Provider == null )
            {
                return BadRequest("Provider or adapter is null");
            }
            switch (adapter.Provider.ProviderType)
            {
                case ProviderType.Ovirt:
                    servicesResponse =
                        await EasyAdmin.Services.Ovirt.ApiService.GetRequest(adapter, "/ovirt-engine/api");
                    break;
                case ProviderType.VMware:
                    var a =
                        await EasyAdmin.Services.VMware.ApiService.GetRequestAsync<string>(adapter, "/status");
                    servicesResponse.isSuccess = a.isSuccess;
                    break;
            }

            if (servicesResponse.isSuccess)
            {
                adapter.IsOK = true;
                adapter.Status = "Подключено";
            }
            else
            {
                adapter.IsOK = false;
                adapter.Status = servicesResponse.errorMessage;
                return BadRequest(servicesResponse.errorMessage);
            }
            _context.Entry(adapter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(true);
        }
        [HttpGet("turnoff/{id}")]
        public async Task<IActionResult> TurnoffAdapter(int id)
        {
            var adapter = _context.Adapters.Include(x => x.Credentials).Include(x => x.Provider).SingleOrDefault(x => x.Id == id);
            if (adapter != null)
            {
                adapter.IsOK = false;

                _context.Entry(adapter).State = EntityState.Modified;
            }
            else
            {
                return BadRequest("adapter == null");
            }
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
