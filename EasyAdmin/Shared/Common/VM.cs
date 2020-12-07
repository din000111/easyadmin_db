using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EasyAdmin.Shared.Interfaces;
using EasyAdmin.Shared.Ovirt;
using Newtonsoft.Json;

namespace EasyAdmin.Shared.Common
{
    public class Vm
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public int AdapterId { get; set; }
        public virtual Adapter Adapter { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        public string Description { get; set; }
        public string ClusterId { get; set; }
        [NotMapped]
        public Cluster Cluster { get; set; }
        public string ClusterName { get; set; }
        public string DatacenterId { get; set; }
        [NotMapped]
        public Datacenter Datacenter { get; set; }
        public string DatacenterName { get; set; }
        [NotMapped]
        public OrganizationUnit Pool{ get; set; }
        public string PoolShortName { get; set; }
        //public Pool Pool { get; set; }
        
        [Range(1, 16, ErrorMessage = "Memory invalid (1-16)GB.")]
        [JsonProperty("memoryGb")]
        public int MemoryGb { get; set; }
 
        [Range(1, 8, ErrorMessage = "CPU invalid (1-8) sockets.")]
        [JsonProperty("cpu")]
        public int Cpu { get; set; }

        [JsonProperty("hddSize")]
        public int HddSize { get; set; }
        [JsonProperty("project")]
        public string Project { get; set; }
        [JsonProperty("services")]
        public string Services { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [NotMapped]
        public User Manager { get; set; }
        [JsonProperty("managerId")]
        public string ManagerId { get; set; }
        [NotMapped]

        public User Admin { get; set; }
        [JsonProperty("adminId")]
        public string AdminId { get; set; }
        [NotMapped]
        public User Owner { get; set; }
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }
        [NotMapped]
        public Network Network{ get; set; }
        [NotMapped]
        public Template Template { get; set; }
        [JsonProperty("auditDate")]
        public string AuditDate { get; set; }
        [JsonProperty("deadLine")]
        public string Deadline { get;  set; }
        [NotMapped]
        public Cost Cost { get; set; }
        //TODO добавлено
        [NotMapped]
        public double TotalCost { get; set; }
        [NotMapped]
        public string HtmlStatus { get; set; }
        //
        public DateTime LastTimeUpdated { get; set; }
        public string FullName { get => $"{Cluster?.ShortName ?? Cluster?.Id.Substring(0,4)}-{PoolShortName}-{Name}"; }

        public static implicit operator Vm(Ovirt.Vm ovirtVm)
        {
            _ = TryParseJson(ovirtVm.Comment, out CommentClass commentClass);
            var hddSize = 0;
            if (ovirtVm.DiskAttachments?.DiskAttachment.All(x=>x.Disk != null) != null)
            {
                hddSize = (int)(ovirtVm.DiskAttachments.DiskAttachment.Sum(x => long.Parse(x.Disk.Actual_size)) / 1073741824);
            }
            var vM = new Vm
            {
                Id = ovirtVm.Id,
                Name = ovirtVm.Name,
                Status = ovirtVm.Status,
                MemoryGb = (int)(ovirtVm.Memory / 1073741824),
                Cpu = ovirtVm.Cpu.Topology.Cores * ovirtVm.Cpu.Topology.Sockets,
                HddSize = hddSize,
                Description = ovirtVm.Description,
                AdminId = commentClass?.Admin,
                ManagerId = commentClass?.Manager,
                OwnerId = commentClass?.Owner,
                Project = commentClass?.Project,
                Services = commentClass?.Services,
                Domain = commentClass?.Domain,
                AuditDate = commentClass?.AuditDate,
                Deadline = commentClass?.Deadline,
                Cluster = ovirtVm.Cluster
            };
            return vM;
        }
        private static bool TryParseJson<T>(string str, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(str, settings);
            return success;
        }
    }

    
}
