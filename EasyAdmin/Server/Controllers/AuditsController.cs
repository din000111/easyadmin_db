using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAdmin.Server.Services.Audit;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController : Controller
    {
        private readonly EasyAdminContext _context;

        private readonly IAuditService _auditService;

        public AuditsController(EasyAdminContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _auditService.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: api/Audits
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Audit>>> GetAudits()
        {
            return await _context.Audits.Include(x => x.Adapter).ToListAsync();
        }
        [HttpGet]
        public IEnumerable<Audit> GetAuditsSafe()
        {
            var audits = _context.Audits.Include(x => x.Adapter);
            foreach (var audit in audits)
            {
                yield return audit;
            }
        }
        [HttpGet]
        [Route("run")]
        public async Task<ActionResult<IEnumerable<Audit>>> RunAudit()
        {
            await _auditService.RunAuditAsync();
            return Ok();
        }

        // GET: api/Audits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            Audit audit = await _context.Audits.FindAsync(id);

            if (audit == null)
            {
                return NotFound();
            }

            return audit;
        }

        // PUT: api/Audits/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAudit(int id, Audit audit)
        {
            if (id != audit?.Id)
            {
                return BadRequest();
            }

            _context.Entry(audit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Audits
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostAudit(Audit audit)
        {
            var isAlreadyConfigured = _context.Audits.Select(x=> new { x.DatacenterId, x.ClusterId })
                .ToList().Any(x => x.DatacenterId == audit.DatacenterId && x.ClusterId == audit.ClusterId);
            if (isAlreadyConfigured)
            {
                return BadRequest("Настройки аудита для этого датацентра и кластера уже были настроены.");
            }
            _context.Audits.Add(audit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAudit", new { id = audit.Id }, audit);
        }

        // DELETE: api/Audits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Audit>> DeleteAudit(int id)
        {
            Audit audit = await _context.Audits.FindAsync(id);
            if (audit == null)
            {
                return NotFound(false);
            }
            _context.Audits.Remove(audit);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.Id == id);
        }
    }
}
