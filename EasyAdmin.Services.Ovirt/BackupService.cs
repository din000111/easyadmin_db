using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;
using EasyAdmin.Shared.Ovirt.Disk;

namespace EasyAdmin.Services.Ovirt
{
    public static class BackupService
    {
        public static async Task<ServicesResponse> EnableIncrementalBackupAsync(Adapter adapter, Vm vm)
        {
            
            var diskAttachmentsRequest = await DiskService.GetDiskAttachments(adapter, vm);
            if (!diskAttachmentsRequest.isSuccess)
            {
                return new ServicesResponse
                {
                    errorMessage = diskAttachmentsRequest.errorMessage,
                    errorCode = diskAttachmentsRequest.errorCode,
                    resultObject = diskAttachmentsRequest.resultObject,
                    isSuccess = diskAttachmentsRequest.isSuccess
                };
            }
            foreach (var diskAttachment in diskAttachmentsRequest.resultObject.DiskAttachment)
            {
                var newDiskAttachment = new DiskAttachment {
                    Interface = diskAttachment.Interface,
                    Disk = new Disk { 
                        Id = diskAttachment.Disk.Id, 
                        Backup = "incremental" 
                    } 
                };
                
                var enableBackupRequest = await ApiService.PostRequest(adapter, newDiskAttachment, $"ovirt-engine/api/vms/{vm.Id}/diskattachments");
                return enableBackupRequest;
            }
            return new ServicesResponse
            {
                errorMessage = diskAttachmentsRequest.errorMessage,
                errorCode = diskAttachmentsRequest.errorCode,
                resultObject = diskAttachmentsRequest.resultObject,
                isSuccess = diskAttachmentsRequest.isSuccess
            };
        }
    }
}
