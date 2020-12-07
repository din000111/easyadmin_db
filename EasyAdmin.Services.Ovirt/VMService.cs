using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt;
using EasyAdmin.Shared.Ovirt.Datacenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action = EasyAdmin.Shared.Ovirt.Action.Action;
using OvirtStorageDomains = EasyAdmin.Shared.Ovirt.StorageDomains;
using Vm = EasyAdmin.Shared.Ovirt.Vm;


namespace EasyAdmin.Services.Ovirt
{
    public static class VmService
    {
        public static async Task<ServicesResponse> RequestVm(Shared.Common.Vm commonVm, Adapter adapter, string networkId = null, int hddSize = 0, string dcId = null)
        {
            var vMRequested = Vm.CreatedVM(commonVm);
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }

            servicesResponse = await ApiService.PostRequest<Vm>(adapter, vMRequested, "ovirt-engine/api/vms");

            if (!servicesResponse.isSuccess || vMRequested.Template?.Name != "Blank")
            {
                return servicesResponse;
            }
            Vm vM = (Vm)servicesResponse.resultObject;

            if (vM == null)
            {
                throw new Exception("vm var is null but created successfully.");
            }

            ServicesResponse newNicResponse = await NetworkService.CreateNic(vM.Id, networkId, adapter);
            if (!newNicResponse.isSuccess)
            {
                return newNicResponse;
            }

            ServicesResponse storageDomainsRequest = await StorageDomainsService.GetStorageDomains(adapter);
            if (!storageDomainsRequest.isSuccess)
            {
                return storageDomainsRequest;
            }
            OvirtStorageDomains.StorageDomains storageDomains = (OvirtStorageDomains.StorageDomains)storageDomainsRequest.resultObject;
            OvirtStorageDomains.StorageDomain storageDomain = storageDomains.StorageDomain
                .FirstOrDefault(dcs => dcs.Data_centers.Data_center.Id == dcId && dcs.Master && dcs.Available > (hddSize * 20 ^ 30) && dcs.Type == "data");


            ServicesResponse newDiskResponse = await DiskService.CreateDisk(vM, hddSize, storageDomain?.Id, adapter);

            return newDiskResponse;
        }

        public static async Task<ServicesResponse> GetVMs(Adapter adapter)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }
            servicesResponse = await ApiService.GetRequest(adapter, typeof(Vms), "ovirt-engine/api/vms");
            servicesResponse.resultObject = (Vms)servicesResponse.resultObject;

            return servicesResponse;
        }
        public static async Task<ServicesResponse> GetFullVms(Adapter adapter)
        {
            var servicesResponse = new ServicesResponse();
            ServicesResponse vmsResponse = await GetVMs(adapter);
            if (!vmsResponse.isSuccess)
            {
                return servicesResponse;
            }

            List<Vm> ovirtVms = ((Vms)vmsResponse.resultObject).Vm;

            ServicesResponse diskResponse = await DiskService.GetDisks(adapter);
            if (!vmsResponse.isSuccess)
            {
                return servicesResponse;
            }

            Shared.Ovirt.Disk.Disks ovirtDisks = (Shared.Ovirt.Disk.Disks)diskResponse.resultObject;

            foreach (Vm ovirtVm in ovirtVms)
            {
                ServicesResponse ovirtVmDiskAttachmentResponse = await ApiService.GetRequest(adapter,
                    typeof(Shared.Ovirt.Disk.DiskAttachments),
                    ovirtVm.Link.SingleOrDefault(l => l.Rel == "diskattachments")?.Href);

                if (!ovirtVmDiskAttachmentResponse.isSuccess)
                {
                    break;
                }
                ovirtVm.DiskAttachments = (Shared.Ovirt.Disk.DiskAttachments)ovirtVmDiskAttachmentResponse.resultObject;
                ovirtVm.DiskAttachments.DiskAttachment.ForEach(x => x.Disk = ovirtDisks.Disk.SingleOrDefault(y => y.Id == x.Disk.Id));
            }

            List<Shared.Ovirt.Datacenter.DataCenter> datacenters = ((DataCenters)(await DatacenterService.GetDatacenters(adapter)).resultObject).DataCenter;
            List<Shared.Ovirt.Cluster> clusters = ((Clusters)(await ClusterService.GetClusters(adapter)).resultObject).Cluster;

            List<Shared.Common.Vm> commonVMs = ovirtVms.ConvertAll(x => (Shared.Common.Vm)x);
            
            foreach (var vm in commonVMs)
            {
                vm.Adapter = adapter;
                vm.AdapterId = adapter.Id;
                vm.Cluster = clusters.SingleOrDefault(x => x.Id == vm.Cluster.Id);
                vm.ClusterId = vm.Cluster.Id;
                vm.Cluster.Datacenter = datacenters.SingleOrDefault(x => x.Id == vm.Cluster.Datacenter.Id);
                vm.DatacenterId = vm.Cluster.Datacenter.Id;
                vm.ClusterName = vm.Cluster.Name;
                vm.DatacenterName = vm.Cluster.Datacenter.Name;
            }
            servicesResponse.isSuccess = true;
            servicesResponse.resultObject = commonVMs;

            return servicesResponse;
        }

        public static async Task<ServicesResponse> ShutdownVm(Adapter adapter, string vmId)
        {
            Action action = new Action();
            ServicesResponse servicesResponse = await ApiService.PostRequest<Action>(adapter, action, $"ovirt-engine/api/vms/{vmId}/shutdown");
            action = (Action)servicesResponse.resultObject;
            servicesResponse.resultObject = action.Status;
            return servicesResponse;
        }
        public static async Task<ServicesResponse> StartVm(Adapter adapter, string vmId)
        {
            Action action = new Action();
            ServicesResponse servicesResponse = await ApiService.PostRequest<Action>(adapter, action, $"ovirt-engine/api/vms/{vmId}/start");
            action = (Action)servicesResponse.resultObject;
            servicesResponse.resultObject = action.Status;
            return servicesResponse;
        }
        public static async Task<ServicesResponse> ChangeState(Adapter adapter, string vmId, string shutOrStart)
        {
            Action action = new Action();
            ServicesResponse servicesResponse = await ApiService.PostRequest<Action>(adapter, action, $"ovirt-engine/api/vms/{vmId}/{shutOrStart}");
            action = (Action)servicesResponse.resultObject;
            servicesResponse.resultObject = action.Status;
            return servicesResponse;
        }

        public static async Task<ServicesResponse> RemoveVm(Adapter adapter, string vmId)
        {
            ServicesResponse servicesResponse = await ApiService.DeleteRequest(adapter, typeof(Action), $"ovirt-engine/api/vms/{vmId}");
            Action action = (Action)servicesResponse.resultObject;
            servicesResponse.resultObject = action.Status;
            return servicesResponse;
        }

        public static async Task<ServicesResponse> UpdateVm(Adapter adapter, Shared.Common.Vm vm)
        {
            var ovirtVm = Vm.UpdatedVM(vm);
            var servicesResponse = await ApiService.PutRequest<Vm, Vm>(adapter, ovirtVm, $"ovirt-engine/api/vms/{vm.Id}");
            return servicesResponse;
        }

        public static async Task<ServicesResponse<Shared.Common.GraphicsConsole>> GetVmConsole(Adapter adapter, string vmId)
        {
            var servicesResponse = await ApiService.GetRequest2<GraphicsConsoles>(adapter, $"ovirt-engine/api/vms/{vmId}/graphicsconsoles/");
            if (!servicesResponse.isSuccess)
            {
                return new ServicesResponse<Shared.Common.GraphicsConsole> {
                    isSuccess = servicesResponse.isSuccess,
                    errorCode = servicesResponse.errorCode,
                    errorMessage = servicesResponse.errorMessage
                };
            }
            var graphicsConsoles = servicesResponse.resultObject;
            var consoleLink = graphicsConsoles.GraphicsConsole.Where(x => x.Protocol == "spice").SingleOrDefault()?.Href;
            var servicesResponse2 = await ApiService.PostRequest2(adapter, new Action(), $"{consoleLink}/remoteviewerconnectionfile");
            if (!servicesResponse2.isSuccess)
            {
                return new ServicesResponse<Shared.Common.GraphicsConsole>
                {
                    isSuccess = servicesResponse2.isSuccess,
                    errorCode = servicesResponse2.errorCode,
                    errorMessage = servicesResponse2.errorMessage
                };
            }
            var file = servicesResponse2.resultObject;
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(file.RemoteViewerConnectionFile);
            var console = new Shared.Common.GraphicsConsole()
            {
                Type = GraphicsConsoleType.File,
                Base64ConsoleFile = Convert.ToBase64String(fileBytes)
            };
            return new ServicesResponse<Shared.Common.GraphicsConsole>()
            {
                isSuccess = true,
                resultObject = console
            };
        }
    }

}
