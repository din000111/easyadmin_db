using EasyAdmin.Services.Ovirt;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyAdmin.Server.Services.Authentication;
using EasyAdmin.Server.Services.Mailer;
using OvirtVM = EasyAdmin.Shared.Ovirt;
using Microsoft.Extensions.DependencyInjection;
using EasyAdmin.Server.Services.BackgroundTasking;
using Microsoft.Extensions.Logging;
using GridMvc.Server;
using EasyAdmin.Client.Pages;
using EasyAdmin.Client.Pages.VmPages;
using Microsoft.Extensions.Caching.Memory;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VmController : Controller
    {
        private readonly EasyAdminContext _context;
        private readonly IMailerService _mailer;
        private readonly IAuthenticationService _authService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<VmController> _logger;
        private readonly IMemoryCache _cache;

        public IBackgroundTaskQueue Queue { get; }

        public VmController(EasyAdminContext context, IMailerService mailer, IAuthenticationService authService, IServiceScopeFactory serviceScopeFactory,
            IBackgroundTaskQueue taskQueue, ILogger<VmController> logger, IMemoryCache cache)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            _context = context;
            _mailer = mailer;
            _authService = authService;
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _cache = cache;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _mailer.Dispose();
                _authService.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<Vm>> Get(int id)
        {
            _logger.LogInformation($"REQUEST : {Request.Path}");
            _logger.LogInformation($"ID : {id}");
            List<Vm> vms = new List<Vm>();
            List<User> users;
            if ((List<Vm>)_cache.Get(0) != null && (List<User>)_cache.Get(1) != null && id == 0)
            {
                vms = (List<Vm>)_cache.Get(0);
                users = ((List<User>)_cache.Get(1));
                _logger.LogInformation($"CACHE VMS : {vms.Count}");
                _logger.LogInformation($"CACHE USERS: {users.Count}");
                return vms;
            }
            else
            {
                _logger.LogInformation($"CACHE NO : {vms.Count}");
                var dbVms = await _context.Vms.ToListAsync();
                var ous = await _context.OrganizationUnits.ToListAsync();
                var adapters = await _context.Adapters.Where(x => x.IsOK).Include(x => x.Provider).Include(a => a.Credentials).ToListAsync();
                users = _authService.GetUsersList();
                foreach (Adapter adapter in adapters)
                {
                    if (!adapter.IsOK)
                    {
                        break;
                    }
                    switch (adapter.Provider.Id)
                    {
                        case (int)ProviderType.Ovirt:
                            ServicesResponse servicesResponse = await VmService.GetVMs(adapter);
                            OvirtVM.Vms ovirtVms;
                            if (!servicesResponse.isSuccess)
                            {
                                break;
                            }
                            ovirtVms = (OvirtVM.Vms)servicesResponse.resultObject;

                            List<Vm> commonVMs = ovirtVms.Vm.ConvertAll(x => (Vm)x);
                            // хз зачем
                            commonVMs.ForEach(x => x.Adapter = adapter);
                            vms.AddRange(commonVMs);
                            adapter.Provider = null;
                            break;
                        case (int)ProviderType.VMware:
                            var response = await EasyAdmin.Services.VMware.VmService.GetVmListAsync(adapter);
                            if (response.isSuccess)
                            {
                                var vmwareVm = response.resultObject;
                                vmwareVm.ForEach(x => x.Adapter = adapter);
                                vms.AddRange(vmwareVm);
                            }
                            // хз зачем но добавлю как выше
                            adapter.Provider = null;
                            break;
                    }
                }

                var isAdmin = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role && x.Value == "Admin") != null;
                foreach (var vm in vms)
                {
                    var dbVm = dbVms.SingleOrDefault(x => x.Id == vm.Id);
                    if (dbVm == null)
                    {
                        //_context.Vms.Add(vm);
                    }
                    else  // if (dbVm.LastTimeUpdated.AddDays(1) < DateTime.Today)
                    {
                        vm.HddSize = dbVm.HddSize;
                    }
                    var haveAccess = new List<string>() { vm.AdminId, vm.OwnerId, vm.ManagerId };
                    var claims = User.Claims.Select(x => x.Value).ToList();
                    if (isAdmin || claims.Intersect(haveAccess).Any())
                    {
                        vm.PoolShortName = ous.FirstOrDefault(p => vm.Name.Contains(p.PoolShortName))?.PoolShortName ?? ous.Single(d => d.Id == 1).PoolShortName;

                        vm.Admin = users.SingleOrDefault(x => x.Sam == vm.AdminId);
                        vm.Owner = users.SingleOrDefault(x => x.Sam == vm.OwnerId);
                        vm.Manager = users.SingleOrDefault(x => x.Sam == vm.ManagerId);
                    }
                    else
                    {
                        vms.Remove(vm);
                    }


                }
                _cache.Remove(0);
                _cache.Set(0, vms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                _cache.Set(1, users, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                return vms;
            }
            //await _context.SaveChangesAsync();
        }
        [HttpGet]
        [Route("grid")]
        public async Task<ActionResult> Grid()
        {
            List<Vm> vms = (List<Vm>)_cache.Get(0);
            foreach (var vm in vms)
            {
                switch (vm.Status)
                {
                    case "up":
                        vm.HtmlStatus = "<span class='fas fa-arrow-circle-up text-success' title='Включена' aria-hidden='true'></span>";
                        break;
                    case "down":
                        vm.HtmlStatus = "<span class='fas fa-arrow-circle-down text-danger' title='Выключена' aria-hidden='true'></span>";
                        break;
                    case "poweredOn":
                        vm.HtmlStatus = "<span class='fas fa-arrow-circle-up text-success' title='Включена' aria-hidden='true'></span>";
                        break;
                    case "poweredOff":
                        vm.HtmlStatus = "<span class='fas fa-arrow-circle-down text-danger' title='Выключена' aria-hidden='true'></span>";
                        break;
                    case "powering_down":
                        vm.HtmlStatus = "<span class='fas fa-angle-double-down text-danger text-center' title='Выключается' aria-hidden='true'></span>";
                        break;
                    case "image_locked":
                        vm.HtmlStatus = "<span class='fas fa-lock text-danger text-center' title='Машина создается' aria-hidden='true'></span>";
                        break;
                    case "powering_up":
                        vm.HtmlStatus = "<span class='fas fa-angle-double-up text-success text-center' title='Включается' aria-hidden='true'></span>";
                        break;
                    case "wait_for_launch":
                        vm.HtmlStatus = "<span class='fas fa-angle-double-up text-success text-center' title='Ожидание запуска' aria-hidden='true'></span>";
                        break;
                    case "not_responding":
                        vm.HtmlStatus = "<span class='fas fa-ban text-danger text-center' title='Включается' aria-hidden='true'></span>";
                        break;
                    case "paused":
                        vm.HtmlStatus = "<span class='fas fa-pause text-warning text-center' title='Пауза' aria-hidden='true'></span>";
                        break;
                    default:
                        vm.HtmlStatus = "<i class='fas fa-spinner fa-pulse'></i>";
                        break;
                }
            }
            var server = new GridServer<Vm>(vms, Request.Query, true, "vmlistgrid", VMList.Columns, 10)
                .ChangePageSize(true)
            .Sortable()
            .Filterable()
            .Searchable()
            .Selectable(true);
            return Ok(server.ItemsToDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> RequestVm(Vm newVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }
            if (newVm?.Cluster?.Datacenter?.Adapter == null)
            {
                return BadRequest("Неверно заполнены поля");
            }
            #region CheckResponsibles
            // Проверка на корректность задания ответственных: администратора, менеджера и владельца
            var responsibles = new List<User>() { newVm.Admin, newVm.Manager, newVm.Owner };
            responsibles = responsibles.GroupBy(x => x.Sam).Select(x => x.FirstOrDefault()).ToList();
            List<User> users = _authService.GetUsersList(responsibles);

            if (responsibles.Count != users.Count)
            {
                return BadRequest("Неверно заполнены поля ответственных за ВМ. Пожалуйста, выберите тех, кто есть в списке");
            }

            #endregion

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK).Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == newVm.Cluster.Datacenter.Adapter.Id)
                .SingleOrDefaultAsync();
            var auditDays = (await _context.Audits.SingleOrDefaultAsync(x => x.AdapterId == adapter.Id && x.DatacenterId == newVm.Cluster.Datacenter.Id && x.ClusterId == newVm.Cluster.Id))?.AuditDays;
            if (auditDays != null)
            {
                newVm.AuditDate = DateTime.Now.ToShortDateString();
                newVm.Deadline = DateTime.Now.AddDays((double)auditDays).ToShortDateString();
            }
            var user = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name).Value;
            switch (adapter.Provider.Id)
            {
                case (int)ProviderType.Ovirt:
                    ServicesResponse newVmResponse = await VmService.RequestVm(newVm, adapter, newVm.Network.Id, newVm.HddSize, newVm.Cluster.Datacenter.Id);
                    if (!newVmResponse.isSuccess)
                    {
                        return StatusCode(newVmResponse.errorCode, newVmResponse.errorMessage);
                    }
                    break;
                case (int)ProviderType.VMware:
                    var task = await SendVmRequest(adapter, newVm, user);
                    TrackBackendTask(adapter, task);
                    break;
            }

            _mailer.NotifyVmRequested(users, newVm);

            return CreatedAtAction("RequestVm", newVm);
        }

        private async Task<BackendTask> SendVmRequest(Adapter adapter, Vm newVm, string username)
        {
            var response = await EasyAdmin.Services.VMware.VmService.RequestVm(adapter, newVm);
            if (!response.isSuccess)
            {
                _logger.LogError(string.Format("{0}:{1}", response.errorCode, response.errorMessage));
            }

            var task = response.resultObject;
            task.IsVisible = true;
            task.UserId = username;
            task.AdapterId = adapter.Id;
            task.Status = "Запрошено создание ВМ";
            task.RelatedEntityType = typeof(Vm).ToString();
            task.RelatedEntityName = newVm.FullName;
            _context.BackendTasks.Add(response.resultObject);
            await _context.SaveChangesAsync();
            return task;
        }

        private void TrackBackendTask(Adapter adapter, BackendTask task)
        {
            _logger.LogInformation("Sending VM request to backend");
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<EasyAdminContext>();


                    while (task.State != "success" && task.State != "error")
                    {
                        var checkTaskStateResponse = await EasyAdmin.Services.VMware.TaskService.GetTaskState(adapter, task);
                        if (checkTaskStateResponse.isSuccess)
                        {
                            var newTaskState = checkTaskStateResponse.resultObject;
                            task.State = newTaskState.State;
                            task.Result = newTaskState.Result;
                            task.Status = "Виртуальная машина успешно создана";
                        }
                        else
                        {
                            task.State = "error";
                            task.Result = task.Status = "Ошибка обновления статуса запроса";
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                    context.Entry(task).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Vm created successfully");


                    //var mailer = scopedServices.GetRequiredService<IMailerService>();
                };
            });
        }

        [HttpPost]
        [Route("shutdown")]
        public async Task<IActionResult> ShutdownVm(Vm vM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vM?.Adapter == null)
            {
                return BadRequest();
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vM.Adapter.Id)
                .SingleOrDefaultAsync();
            ServicesResponse shutdownVmResponse = new ServicesResponse();
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    shutdownVmResponse = await VmService.ShutdownVm(adapter, vM.Id);
                    if (shutdownVmResponse.isSuccess)
                    {
                    }
                    else
                    {
                        return BadRequest(shutdownVmResponse.errorMessage);
                    }
                    break;
            }
            return Ok(shutdownVmResponse);
        }
        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> Start(Vm vM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vM?.Adapter == null)
            {
                return BadRequest();
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vM.Adapter.Id)
                .SingleOrDefaultAsync();
            ServicesResponse shutdownVmResponse = new ServicesResponse();
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    shutdownVmResponse = await VmService.StartVm(adapter, vM.Id);
                    if (shutdownVmResponse.isSuccess)
                    {
                    }
                    else
                    {
                        return BadRequest(shutdownVmResponse.errorMessage);
                    }
                    break;
            }
            return Ok(shutdownVmResponse);
        }
        [HttpPost]
        [Route("changestate")]
        public async Task<IActionResult> ChangeState(Vm vM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vM?.Adapter == null)
            {
                return BadRequest();
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vM.Adapter.Id)
                .SingleOrDefaultAsync();
            ServicesResponse changeStateVmResponse = new ServicesResponse();

            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":

                    string shutOrStart = vM.Status == "down" ? "start" : "shutdown";
                    changeStateVmResponse = await VmService.ChangeState(adapter, vM.Id, shutOrStart);
                    if (changeStateVmResponse.isSuccess)
                    {
                        _logger.LogInformation($"CHANGE STATUS : {vM.Status}");
                        _logger.LogInformation($"CHANGE STATUS : {changeStateVmResponse.resultObject}");

                    }
                    else
                    {
                        return BadRequest(changeStateVmResponse.errorMessage);
                    }
                    break;
            }
            return Ok(changeStateVmResponse);
        }
        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> Remove(Vm vM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vM?.Adapter == null)
            {
                return BadRequest();
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vM.Adapter.Id)
                .SingleOrDefaultAsync();
            ServicesResponse shutdownVmResponse = new ServicesResponse();
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    shutdownVmResponse = await VmService.RemoveVm(adapter, vM.Id);
                    if (!shutdownVmResponse.isSuccess)
                    {
                        return BadRequest(shutdownVmResponse.errorMessage);
                    }
                    break;
            }
            return Ok(shutdownVmResponse);
        }
        [HttpPost]
        [Route("prolong")]
        public async Task<IActionResult> ProlongVm(Vm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state no valid");
            }
            if (vm?.Adapter == null)
            {
                return BadRequest("Vm adapter is null");
            }

            Audit audit = await _context.Audits.Where(x => x.ClusterId == vm.Cluster.Id).SingleOrDefaultAsync();
            if (audit == null)
            {
                return BadRequest("Настройки аудита для кластера не найдены.");
            }
            vm.AuditDate = DateTime.Now.ToShortDateString();
            vm.Deadline = DateTime.Now.AddDays(audit.AuditDays).ToShortDateString();

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vm.Adapter.Id)
                .SingleOrDefaultAsync();

            ServicesResponse servicesResponse;
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    servicesResponse = await VmService.UpdateVm(adapter, vm);
                    if (!servicesResponse.isSuccess)
                    {
                        return StatusCode(servicesResponse.errorCode, servicesResponse.errorMessage);
                    }
                    return Ok(vm);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<BackResponse>> UpdateVm(Vm vm)
        {
            var backResponse = new BackResponse();
            if (!ModelState.IsValid)
            {
                backResponse.Status = "Model state no valid";
                return BadRequest(backResponse);
            }
            if (vm?.Adapter == null)
            {
                backResponse.Status = "Vm adapter is null";
                return BadRequest(backResponse);
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vm.Adapter.Id)
                .SingleOrDefaultAsync();
            ServicesResponse servicesResponse;
            switch (adapter.Provider.Name.ToLower())
            {
                case "ovirt":
                    servicesResponse = await VmService.UpdateVm(adapter, vm);
                    if (!servicesResponse.isSuccess)
                    {
                        backResponse.IsSuccess = false;
                        backResponse.Status = servicesResponse.errorMessage;
                        backResponse.StatusCode = servicesResponse.errorCode;
                    }
                    else
                    {
                        backResponse.IsSuccess = true;
                    }
                    break;
            }
            return Ok(backResponse);
        }

        [HttpGet]
        [Route("getfull")]
        public async Task<List<Vm>> GetFull()
        {
            List<Vm> vms = await _context.Vms.ToListAsync();
            if (vms.Count == 0)
            {
                await Sync();
                vms = await _context.Vms.ToListAsync();
                return vms;
            }
            return vms;
        }

        [HttpGet]
        [Route("sync")]
        public async Task Sync()
        {
            List<Vm> vms = new List<Vm>();
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Adapter, Credentials> adapters = _context.Adapters.Where(x => x.IsOK).Include(x => x.Provider).Include(a => a.Credentials);
            foreach (Adapter adapter in adapters)
            {
                if (!adapter.IsOK)
                {
                    break;
                }
                switch (adapter.Provider.Name.ToLower())
                {
                    case "ovirt":
                        ServicesResponse servicesResponse = await VmService.GetFullVms(adapter);
                        if (!servicesResponse.isSuccess)
                        {
                            break;
                        }
                        List<Vm> commonVMs = (List<Vm>)servicesResponse.resultObject;

                        vms.AddRange(commonVMs);
                        break;
                }
            }
            var oldVms = await _context.Vms.ToListAsync();
            _context.Vms.RemoveRange(oldVms);

            vms.ConvertAll(x => x.LastTimeUpdated = DateTime.Now);
            _context.Vms.AddRange(vms);
            //foreach (var vm in vms)
            //{
            //    var entity = _context.Vms.Find(vm.Id);
            //    if (entity == null)
            //    {
            //        _context.Vms.Add(vm);
            //    }
            //    else
            //    {
            //        _context.Entry(entity).CurrentValues.SetValues(vm);
            //    }                
            //}

            await _context.SaveChangesAsync();
        }
        [HttpPost]
        [Route("console")]
        public async Task<IActionResult> GetConsole(Vm vM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vM?.Adapter == null)
            {
                return BadRequest();
            }

            Adapter adapter = await _context.Adapters.Where(x => x.IsOK)
                .Include(c => c.Credentials)
                .Include(p => p.Provider)
                .Where(a => a.Id == vM.Adapter.Id)
                .SingleOrDefaultAsync();

            var servicesResponse = new ServicesResponse<GraphicsConsole>();
            switch (adapter.Provider.Id)
            {
                case (int)ProviderType.Ovirt:
                    servicesResponse = await VmService.GetVmConsole(adapter, vM.Id);
                    break;
                case (int)ProviderType.VMware:
                    servicesResponse = await EasyAdmin.Services.VMware.VmService.GetConsole(adapter, vM);
                    break;
            }
            if (!servicesResponse.isSuccess)
            {
                return BadRequest(servicesResponse.errorMessage);
            }
            return Ok(servicesResponse.resultObject);
        }
    }
}