using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAdmin.Server.Services.Authentication;
using EasyAdmin.Server.Services.Mailer;
using EasyAdmin.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace EasyAdmin.Server.Services.Audit
{
    public interface IAuditService
    {
        Task RunAuditAsync();
        Task DoWork(CancellationToken stoppingToken);
        void Dispose();
    }

    public class AuditService : IAuditService
    {
        private readonly EasyAdminContext _context;
        private readonly IMailerService _mailer;
        private readonly IAuthenticationService _authService;
        private DateTime _date;
        public AuditService(EasyAdminContext context, IMailerService mailer, IAuthenticationService authService)
        {
            _context = context;
            _mailer = mailer;
            _authService = authService;
            _date = DateTime.Today;
            TimeSpan ts = new TimeSpan(13, 0, 0);
            _date = _date.Date + ts;
        }

        public void Dispose()
        {
            _context.Dispose();
            _mailer.Dispose();
            _authService.Dispose();
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dateNow = DateTime.Now;
                TimeSpan ts;
                if (_date > dateNow)
                    ts = _date - dateNow;
                else
                {
                    _date = _date.AddDays(1);
                    ts = _date - dateNow;
                }

                await Task.Delay(ts, stoppingToken);

                await RunAuditAsync();
            }
        }
        public async Task RunAuditAsync()
        {
            List<Adapter> adapters = await _context.Adapters.Where(x => x.IsOK).Include(x => x.Provider).Include(y => y.Credentials).ToListAsync();
            List<Shared.Common.Audit> audits = await _context.Audits.ToListAsync();
            if (audits == null)
            {
                return;
            }
            foreach (Adapter adapter in adapters)
            {
                switch (adapter.Provider.Name.ToLower())
                {
                    case "ovirt":
                        ServicesResponse vmsResponse = await EasyAdmin.Services.Ovirt.VmService.GetVMs(adapter);
                        if (!vmsResponse.isSuccess)
                        {
                            break;
                        }

                        List<Shared.Ovirt.Vm> ovirtVms = ((Shared.Ovirt.Vms)vmsResponse.resultObject).Vm;

                        List<Shared.Ovirt.Cluster> clusters = ((Shared.Ovirt.Clusters)(await EasyAdmin.Services.Ovirt.ClusterService.GetClusters(adapter)).resultObject).Cluster;

                        List<Vm> commonVMs = ovirtVms.ConvertAll(x => (Vm)x);

                        foreach (Vm vm in commonVMs)
                        {
                            vm.Cluster = clusters.SingleOrDefault(x => x.Id == vm.Cluster.Id);
                            var responsibles = new List<string>() { vm.AdminId, vm.OwnerId, vm.ManagerId };
                            responsibles = responsibles.Distinct().ToList();
                            List<User> users = await _authService.GetUsersAsync(responsibles);
                            if (vm.Deadline != null && DateTime.Parse(vm.Deadline).AddDays(1) < DateTime.Now)
                            {
                                AuditActionTypes.EnumAuditActionTypes auditType = audits.SingleOrDefault(x => x.ClusterId == vm.Cluster?.Id && x.DatacenterId == vm.Cluster?.Datacenter?.Id).AuditActionType;
                                switch (auditType)
                                {
                                    case AuditActionTypes.EnumAuditActionTypes.Archive:
                                        await EasyAdmin.Services.Ovirt.VmService.ShutdownVm(adapter, vm.Id);
                                        break;
                                    case AuditActionTypes.EnumAuditActionTypes.Shutdown:
                                        await EasyAdmin.Services.Ovirt.VmService.ShutdownVm(adapter, vm.Id);
                                        break;
                                    case AuditActionTypes.EnumAuditActionTypes.Notify:
                                        _mailer.NotifyVmAudit(users, vm, AuditActionTypes.EnumAuditActionTypes.Notify);
                                        Console.WriteLine($"{vm.Name} audit date passed!");
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
