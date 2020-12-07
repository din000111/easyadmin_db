using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationUnitsController : Controller
    {
        private readonly EasyAdminContext _context;

        public OrganizationUnitsController(EasyAdminContext context)
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

        // GET: api/OrganizationUnits
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<OrganizationUnit>>> GetOrganizationUnits()
        {
            return await _context.OrganizationUnits.ToListAsync();
        }
        [HttpGet]
        [Route("my")]
        public async Task<ActionResult<IEnumerable<OrganizationUnit>>> GetMyOrganizationUnits()
        {
            List<OrganizationUnit> ous = await _context.OrganizationUnits.ToListAsync();
            string myDn = User.Claims.ToList().SingleOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
            if (myDn != null)
            {
                myDn = myDn.Split(',', 2)[1];
                OrganizationUnit myOu = ous.SingleOrDefault(x => x.DistinguishedName == myDn);
                if (myOu != null)
                {
                    ous.Clear();
                    ous.Add(myOu);
                    while (ous.Last().Parent != null)
                    {
                        ous.Add(ous.Last().Parent);
                    }
                }
                else
                {
                    ous.RemoveAll(x => x.PoolShortName != "DEF");
                }
            }

            return ous;
        }

        // GET: api/OrganizationUnits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationUnit>> GetOrganizationUnit(int id)
        {
            OrganizationUnit organizationUnit = await _context.OrganizationUnits.FindAsync(id);

            if (organizationUnit == null)
            {
                return NotFound();
            }

            return organizationUnit;
        }

        // PUT: api/OrganizationUnits/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutOrganizationUnit(int id, OrganizationUnit organizationUnit)
        {
            if (id != organizationUnit.Id)
            {
                return BadRequest();
            }

            _context.Entry(organizationUnit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationUnitExists(id))
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

        // POST: api/OrganizationUnits
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult<OrganizationUnit>> PostOrganizationUnit(OrganizationUnit organizationUnit)
        {
            _context.OrganizationUnits.Add(organizationUnit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganizationUnit", new { id = organizationUnit.Id }, organizationUnit);
        }

        // DELETE: api/OrganizationUnits/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrganizationUnit>> DeleteOrganizationUnit(int id)
        {
            OrganizationUnit organizationUnit = await _context.OrganizationUnits.FindAsync(id);
            if (organizationUnit == null)
            {
                return NotFound();
            }

            _context.OrganizationUnits.Remove(organizationUnit);
            await _context.SaveChangesAsync();

            return organizationUnit;
        }

        private bool OrganizationUnitExists(int id)
        {
            return _context.OrganizationUnits.Any(e => e.Id == id);
        }
    }
}
