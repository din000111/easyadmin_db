using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;
using EasyAdmin.Shared.Ovirt.Disk;
using Newtonsoft.Json;

namespace EasyAdmin.Shared.Ovirt
{
    


    [XmlRoot(ElementName = "original_template")]
    public class OriginalTemplate
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
	[XmlRoot(ElementName = "lease")]
	public class Lease
	{
		[XmlElement(ElementName = "storage_domain")]
		public StorageDomain StorageDomain { get; set; }
	}


	[XmlRoot(ElementName = "vm")]
    public class Vm
    {
        private const double KB_IN_GB = 1073741824;

		[XmlElement(ElementName = "actions")]
		public Actions Actions { get; set; }
		[XmlElement(ElementName = "bios")]
		public Bios Bios { get; set; }
		[XmlElement(ElementName = "cluster")]
		public Cluster Cluster { get; set; }
		[XmlElement(ElementName = "comment")]
		public string Comment { get; set; }
		[XmlElement(ElementName = "cpu")]
		public Cpu Cpu { get; set; }
		[XmlElement(ElementName = "cpu_profile")]
		public CpuProfile CpuProfile { get; set; }
		[XmlElement(ElementName = "cpu_shares")]
		public string CpuShares { get; set; }
		[XmlElement(ElementName = "creation_time")]
		public string CreationTime { get; set; }
		[XmlElement(ElementName = "delete_protected")]
		public string DeleteProtected { get; set; }
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "display")]
		public Display Display { get; set; }
		[XmlElement(ElementName = "domain")]
		public Domain Domain { get; set; }
		[XmlElement(ElementName = "fqdn")]
		public string Fqdn { get; set; }
		[XmlElement(ElementName = "guest_operating_system")]
		public GuestOperatingSystem GuestOperatingSystem { get; set; }
		[XmlElement(ElementName = "guest_time_zone")]
		public GuestTimeZone GuestTimeZone { get; set; }
		[XmlElement(ElementName = "high_availability")]
		public HighAvailability HighAvailability { get; set; }
		[XmlElement(ElementName = "host")]
		public Host Host { get; set; }
		[XmlAttribute(AttributeName = "href")]
		public string Href { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "initialization")]
		public Initialization Initialization { get; set; }
		[XmlElement(ElementName = "io")]
		public Io Io { get; set; }
		[XmlElement(ElementName = "large_icon")]
		public LargeIcon LargeIcon { get; set; }
		[XmlElement(ElementName = "lease")]
		public Lease Lease { get; set; }
		[XmlElement(ElementName = "link")]
		public List<Link> Link { get; set; }
		[XmlElement(ElementName = "memory")]
		public double Memory { get; set; }
		[XmlElement(ElementName = "memory_policy")]
		public MemoryPolicy MemoryPolicy { get; set; }
		[XmlElement(ElementName = "migration")]
		public Migration Migration { get; set; }
		[XmlElement(ElementName = "migration_downtime")]
		public string MigrationDowntime { get; set; }
		[XmlElement(ElementName = "multi_queues_enabled")]
		public string MultiQueuesEnabled { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "next_run_configuration_exists")]
		public string NextRunConfigurationExists { get; set; }
		[XmlElement(ElementName = "numa_tune_mode")]
		public string NumaTuneMode { get; set; }
		[XmlElement(ElementName = "origin")]
		public string Origin { get; set; }
		[XmlElement(ElementName = "original_template")]
		public OriginalTemplate OriginalTemplate { get; set; }
		[XmlElement(ElementName = "os")]
		public Os Os { get; set; }
		[XmlElement(ElementName = "placement_policy")]
		public PlacementPolicy PlacementPolicy { get; set; }
		[XmlElement(ElementName = "quota")]
		public Quota Quota { get; set; }
		[XmlElement(ElementName = "run_once")]
		public string RunOnce { get; set; }
		[XmlElement(ElementName = "small_icon")]
		public SmallIcon SmallIcon { get; set; }
		[XmlElement(ElementName = "sso")]
		public Sso Sso { get; set; }
		[XmlElement(ElementName = "start_paused")]
		public string StartPaused { get; set; }
		[XmlElement(ElementName = "start_time")]
		public string StartTime { get; set; }
		[XmlElement(ElementName = "stateless")]
		public string Stateless { get; set; }
		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
		[XmlElement(ElementName = "stop_reason")]
		public string StopReason { get; set; }
		[XmlElement(ElementName = "stop_time")]
		public string StopTime { get; set; }
		[XmlElement(ElementName = "storage_error_resume_behaviour")]
		public string StorageErrorResumeBehaviour { get; set; }
		[XmlElement(ElementName = "template")]
		public Template Template { get; set; }
		[XmlElement(ElementName = "time_zone")]
		public TimeZone TimeZone { get; set; }
		[XmlElement(ElementName = "type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "usb")]
		public Usb Usb { get; set; }
		[XmlIgnore]
		public DiskAttachments DiskAttachments { get; set; }

        public static Vm CreatedVM(Common.Vm commonVm)
        {
            var device = new List<string> {"hd"};
            var vM = new Vm();
            if (commonVm != null)
            {
                double memoryKiB = (double)commonVm.MemoryGb * KB_IN_GB;
                vM = new Vm
                {
                    Id = commonVm.Id,
                    Name = $"{commonVm.Cluster.ShortName}-{commonVm.PoolShortName}-{commonVm.Name}",
                    Memory = memoryKiB,
                    MemoryPolicy = new MemoryPolicy { Max = memoryKiB },
                    Os = new Os { Boot = new Boot { Devices = new Devices { Device = device } } },
                    Cpu = new Cpu { Topology = new Topology { Sockets = commonVm.Cpu, Cores = 1, Threads = 1 } },
                    Template = commonVm.Template != null ? new Template { Name = commonVm.Template?.Name } : null,
                    Cluster = new Cluster { Name = commonVm.Cluster.Name },
                    Comment = JsonConvert.SerializeObject(new CommentClass {
                        Admin = commonVm.Admin?.Sam, 
                        Manager = commonVm.Manager?.Sam, 
                        Owner = commonVm.Owner?.Sam,
                        Domain = commonVm.Domain,
                        Project = commonVm.Project,
                        Services = commonVm.Services,
                        AuditDate = commonVm.AuditDate,
                        Deadline = commonVm.Deadline
                    })
                };                
            }
            return vM;
        }
        public static Vm UpdatedVM(Common.Vm commonVm)
        {
            if (commonVm == null)
            {
                throw new System.ArgumentNullException();
            }
            var vm = new Vm
            {
                Id = commonVm.Id,
                Comment = JsonConvert.SerializeObject(new CommentClass
                {
                    Admin = commonVm.Admin?.Sam,
                    Manager = commonVm.Manager?.Sam,
                    Owner = commonVm.Owner?.Sam,
                    Domain = commonVm.Domain,
                    Project = commonVm.Project,
                    Services = commonVm.Services,
                    AuditDate = commonVm.AuditDate,
                    Deadline = commonVm.Deadline
                }),
                Memory = (double)commonVm.MemoryGb * KB_IN_GB
            };

            return vm;
        }
    }

    public class CommentClass
    {
        public string Admin { get; set; }
        public string Manager { get; set; }
        public string Owner { get; set; }
        public string Project { get; set; }
        public string Services { get; set; }
        public string Domain { get; set; }
        public string AuditDate { get; set; }
        public string Deadline { get; set; }
    }

    [XmlRoot(ElementName = "vms")]
    public class Vms
    {
        [XmlElement(ElementName = "vm")]
        public List<Vm> Vm { get; set; }
    }
}
