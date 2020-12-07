using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//TODO добавлено
using GridMvc.Server;
using Vm = EasyAdmin.Shared.Common.Vm;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CostsController : Controller
    {
        private readonly EasyAdminContext _context;
        ILogger<CostsController> logger;

        public CostsController(EasyAdminContext context,ILogger<CostsController> logger)
        {
            _context = context;
            this.logger = logger;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: api/Costs
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Cost>>> GetCosts()
        {
            return await _context.Costs.Include(x => x.Adapter).ToListAsync();
        }
        [HttpGet]
        public async IAsyncEnumerable<Cost> GetCostsSafe()
        {
            var costs = _context.Costs.Include(x => x.Adapter);
            foreach (var cost in costs)
            {
                yield return cost;
            }
        }
        //TODO изменено
        [HttpGet]
        [Route("billing")]
        public async Task<ActionResult<IEnumerable<Vm>>> Billing()
        {
            //List<Billing> billing = new List<Billing>();            

            List<Cost> costs = await _context.Costs.Include(x => x.Adapter)
                .ThenInclude(c => c.Credentials)
                .Include(a => a.Adapter).ThenInclude(p => p.Provider)
                .ToListAsync();

            //List<Adapter> adapters = await _context.Adapters.Where(x => x.IsOK).Include(p => p.Provider).Include(c => c.Credentials).ToListAsync();
            DbSet<OrganizationUnit> ous = _context.OrganizationUnits;

            var vms = _context.Vms.Include(x => x.Adapter).ToList();
            
                if (vms.Count==0)
                {
                    return NotFound();
                }
            foreach (var vm in vms)
            {
                vm.PoolShortName = ous.FirstOrDefault(p => vm.Name.Contains(p.PoolShortName))?.PoolShortName ?? ous.Single(d => d.Id == 1).PoolShortName;
                //проверка выборки SingleOrDefault:
                try
                {
                    vm.Cost = costs.SingleOrDefault(x => x.DatacenterId == vm.DatacenterId && x.AdapterId == vm.AdapterId);

                }
                //исключение, если в коллекции costs 2 элемента с одинаковыми DatacenterId и/или AdapterId. в этом случае метод SingleOrDefault 
                //завершается с ошибкой InvalidOperationException. Маловероятно, но всё же. (хотя это в базе данных проверяется... тоже...) 
                catch (System.Exception ex)
                {
                    //В этом случае при обработке первого vm операция завершается:
                    return BadRequest();
                }
                //если элемент Cost с соответствующими Id адаптера и датацентра не найден
                //или коллекция costs пуста, метод SingleOrDefault() возвращает null
                if (vm.Cost == null)
                {
                    //при обработке первого элемента операция завершается:
                    return NoContent();
                }

                //если Cost найден, для каждого vm рассчитывается TotalCost
                else
                {
                    vm.TotalCost = (vm.Cpu * vm.Cost.CpuCost) + (vm.MemoryGb * vm.Cost.MemoryCost) + (vm.HddSize * vm.Cost.HddCost);
                }
            }

            return Ok(vms);
        }
        //action to make grid working
        [HttpGet("[action]")]
        public async Task<ActionResult> Grid()
        {
            List<Cost> costs = await _context.Costs.Include(x => x.Adapter)
                .ThenInclude(c => c.Credentials)
                .Include(a => a.Adapter).ThenInclude(p => p.Provider)
                .ToListAsync();
            DbSet<OrganizationUnit> ous = _context.OrganizationUnits;
            var vms = _context.Vms.ToList();
            foreach (var vm in vms)
            {
                vm.PoolShortName = ous.FirstOrDefault(p => vm.Name.Contains(p.PoolShortName))?.PoolShortName ?? ous.Single(d => d.Id == 1).PoolShortName;
                vm.Cost = costs.SingleOrDefault(x => x.DatacenterId == vm.DatacenterId && x.AdapterId == vm.AdapterId);
                vm.TotalCost = (vm.Cpu * vm.Cost.CpuCost) + (vm.MemoryGb * vm.Cost.MemoryCost) + (vm.HddSize * vm.Cost.HddCost);
            }

            var server = new GridServer<Vm>(vms, Request.Query, true, "grid", Client.Pages.BillingPage.Columns, 7)
                .Sortable()
                .Searchable()
                .Filterable()
                .ChangePageSize(true)
                .Selectable(true)
                .WithGridItemsCount();
            return Ok(server.ItemsToDisplay);
        }

        // GET: api/Costs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cost>> GetCost(int id)
        {
            Cost cost = await _context.Costs.FindAsync(id);

            if (cost == null)
            {
                return NotFound();
            }

            return cost;
        }

        // PUT: api/Costs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCost(int id, Cost cost)
        {
            if (id != cost.Id)
            {
                return BadRequest();
            }

            _context.Entry(cost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostExists(id))
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

        // POST: api/Costs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostCost(Cost cost)
        {
            _context.Costs.Add(cost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCost", new { id = cost.Id }, cost);
        }

        // DELETE: api/Costs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cost>> DeleteCost(int id)
        {
            Cost cost = await _context.Costs.FindAsync(id);
            if (cost == null)
            {
                return NotFound(false);
            }
            cost.Adapter = null;

            _context.Costs.Remove(cost);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        private bool CostExists(int id)
        {
            return _context.Costs.Any(e => e.Id == id);
        }
    }
}
