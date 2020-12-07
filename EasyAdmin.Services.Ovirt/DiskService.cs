using EasyAdmin.Shared.Common;
using System.Threading.Tasks;
using OvirtDisk = EasyAdmin.Shared.Ovirt.Disk;
using Vm = EasyAdmin.Shared.Ovirt.Vm;

namespace EasyAdmin.Services.Ovirt
{
    public static class DiskService
    {
        public static async Task<ServicesResponse> CreateDisk(Vm vM, double hddSize, string storageDomainId, Adapter adapter)
        {
            OvirtDisk.DiskAttachment diskAttachments = new OvirtDisk.DiskAttachment
            {
                Bootable = false,
                Active = true,
                Interface = "virtio",
                Disk = new OvirtDisk.Disk
                {
                    Description = $"{vM.Name} virtual disk",
                    Format = "cow",
                    Name = "vmdisk",

                    // 1 073 741 824‬ * GB  = 2^30 KBytes
                    ProvisionedSize = hddSize * 1073741824,
                    StorageDomains = new OvirtDisk.StorageDomains
                    {
                        StorageDomain = new OvirtDisk.StorageDomain
                        {
                            Id = storageDomainId
                        }
                    }
                }
            };

            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }

            servicesResponse = await ApiService.PostRequest<OvirtDisk.DiskAttachment>(adapter, diskAttachments, $"ovirt-engine/api/vms/{vM.Id}/diskattachments");

            return servicesResponse;
        }

        public static async Task<ServicesResponse> GetDisks(Adapter adapter)
        {
            ServicesResponse servicesResponse = new ServicesResponse();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }
            servicesResponse = await ApiService.GetRequest(adapter, typeof(OvirtDisk.Disks), $"ovirt-engine/api/disks");

            return servicesResponse;
        }
        public static async Task<ServicesResponse<OvirtDisk.DiskAttachments>> GetDiskAttachments(Adapter adapter, Shared.Common.Vm vm)
        {
            var servicesResponse = new ServicesResponse<OvirtDisk.DiskAttachments>();
            if (adapter?.Credentials == null)
            {
                servicesResponse.errorCode = 500;
                servicesResponse.errorMessage = "Adapter or credentials is null";
                return servicesResponse;
            }
            servicesResponse = await ApiService.GetRequest2<OvirtDisk.DiskAttachments>(adapter, $"ovirt-engine/api/vms/{vm.Id}/diskattachments");

            return servicesResponse;
        }
    }
}
