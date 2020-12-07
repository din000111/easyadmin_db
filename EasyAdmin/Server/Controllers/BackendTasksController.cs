using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyAdmin.Server;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BackendTasksController : ControllerBase
    {
        private readonly EasyAdminContext _context;

        public BackendTasksController(EasyAdminContext context)
        {
            _context = context;
        }

        // GET: api/BackendTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BackendTask>>> GetBackendTasks()
        {
            var all = await _context.BackendTasks.ToListAsync();
            return await _context.BackendTasks.Where(x => x.UserId == User.Identity.Name && x.IsVisible == true).ToListAsync();
        }

        //PUT: api/BackendTasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> HideBackendTask(BackendTask backendTask)
        {
            if (backendTask?.Id == null && backendTask?.UserId == User.Identity.Name)
            {
                return BadRequest();
            }
            backendTask.IsVisible = false;
            _context.Entry(backendTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BackendTaskExists(backendTask.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(true);
        }

        // PUT: api/BackendTasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBackendTask(int id, BackendTask backendTask)
        //{
        //    if (id != backendTask.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(backendTask).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BackendTaskExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // GET: api/BackendTasks/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BackendTask>> GetBackendTask(int id)
        //{
        //    var backendTask = await _context.BackendTasks.FindAsync(id);

        //    if (backendTask == null)
        //    {
        //        return NotFound();
        //    }

        //    return backendTask;
        //}
        // POST: api/BackendTasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<BackendTask>> PostBackendTask(BackendTask backendTask)
        //{
        //    _context.BackendTasks.Add(backendTask);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBackendTask", new { id = backendTask.Id }, backendTask);
        //}

        //// DELETE: api/BackendTasks/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<BackendTask>> DeleteBackendTask(int id)
        //{
        //    var backendTask = await _context.BackendTasks.FindAsync(id);
        //    if (backendTask == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BackendTasks.Remove(backendTask);
        //    await _context.SaveChangesAsync();

        //    return backendTask;
        //}

        private bool BackendTaskExists(int id)
        {
            return _context.BackendTasks.Any(e => e.Id == id);
        }
    }
}
