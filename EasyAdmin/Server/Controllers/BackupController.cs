using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EasyAdmin.Server.Services.Authentication;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly EasyAdminContext _context;
        private readonly IAuthenticationService _authService;
        public BackupController(EasyAdminContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(Vm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }
            if (vm?.Adapter == null)
            {
                return BadRequest("Неверно заполнены поля");
            }
            
            Adapter adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vm.Adapter.Id)
                .SingleOrDefaultAsync();

            switch (adapter.Provider.Id)
            {
                case 1:
                    var disks = EasyAdmin.Services.Ovirt.BackupService.EnableIncrementalBackupAsync(adapter, vm);
                    return Ok(JsonSerializer.Serialize(disks));
                default:
                    break;
            }
            return NoContent();
        }

        //// PUT: api/Backup/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
