using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAdmin.Shared.Common;
namespace EasyAdmin.Client.Services
{
    public interface IApiService
    {
        //Adapters
        Task<ApiResponse<List<Adapter>>> GetAdaptersListAsync();
        Task<ApiResponse<Adapter>> GetAdapterAsync();
        Task<ApiResponse<Adapter>> CreateAdapterAsync(Adapter adapter);
        Task<ApiResponse<Adapter>> UpdateAdapterAsync(Adapter adapter);
        Task<ApiResponse<bool>> TestAdapterAsync(Adapter adapter);
        Task<ApiResponse<bool>> TurnOffAdapterAsync(Adapter adapter);
        Task<ApiResponse<bool>> DeleteAdapterAsync(Adapter adapter);
        //Providers
        Task<ApiResponse<List<Provider>>> GetProvidersListAsync();
        //Audits
        Task<ApiResponse<List<Audit>>> GetAuditsListAsync();
        Task<ApiResponse<Audit>> CreateAuditAsync(Audit audit);
        Task<ApiResponse<Audit>> UpdateAuditAsync(Audit audit);
        Task<ApiResponse<bool>> DeleteAuditAsync(Audit audit);
        //Datacenters
        Task<ApiResponse<List<Datacenter>>> GetDatacentersListAsync(Adapter adapter);
        //Clusters
        Task<ApiResponse<List<Cluster>>> GetClustersListAsync(Datacenter datacenter);
        //Costs
        Task<ApiResponse<List<Cost>>> GetCostsListAsync();
        Task<ApiResponse<Cost>> CreateCostAsync(Cost cost);
        Task<ApiResponse<Cost>> UpdateCostAsync(Cost cost);
        Task<ApiResponse<bool>> DeleteCostAsync(Cost cost);
        //Vm
        Task<ApiResponse<List<Vm>>> GetVmListAsync();
        Task<ApiResponse<Vm>> CreateVmAsync(Vm vm);
        Task<ApiResponse<Vm>> UpdateVmAsync(Vm vm);
        Task<ApiResponse<Vm>> ProlongVmAsync(Vm vm);
        //BackendTasks
        Task<ApiResponse<List<BackendTask>>>GetBackendTasksListAsync();
        Task<ApiResponse<bool>> HideBackendTask(BackendTask backendTask);
        //Misc
        Task<ApiResponse<List<User>>> GetUsersListAsync();
        Task<ApiResponse<List<OrganizationUnit>>> GetOUsListAsync();
        Task<ApiResponse<OrganizationUnit>> CreateOUAsync(OrganizationUnit organizationUnit);
        Task<ApiResponse<List<Template>>> GetTemplatesListAsync(Cluster cluster);
        Task<ApiResponse<List<Network>>> GetNetworksListAsync(Cluster cluster);
        Task<ApiResponse<string>> GetVmDiskAttachmentsAsync(Vm vm);
    }
}
