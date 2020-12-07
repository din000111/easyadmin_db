using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;

namespace EasyAdmin.Shared.Ovirt
{
    [XmlRoot(ElementName = "base_template")]
    public class BaseTemplate
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    
    [XmlRoot(ElementName = "template")]
    public class Template
    {
        [XmlElement(ElementName = "actions")]
        public Actions Actions { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }
        [XmlElement(ElementName = "link")]
        public List<Link> Link { get; set; }
        [XmlElement(ElementName = "bios")]
        public Bios Bios { get; set; }
        [XmlElement(ElementName = "cpu")]
        public Cpu Cpu { get; set; }
        [XmlElement(ElementName = "cpu_shares")]
        public string CpuShares { get; set; }
        [XmlElement(ElementName = "creation_time")]
        public string CreationTime { get; set; }
        [XmlElement(ElementName = "delete_protected")]
        public string DeleteProtected { get; set; }
        [XmlElement(ElementName = "display")]
        public Display Display { get; set; }
        [XmlElement(ElementName = "high_availability")]
        public HighAvailability HighAvailability { get; set; }
        [XmlElement(ElementName = "io")]
        public Io Io { get; set; }
        [XmlElement(ElementName = "large_icon")]
        public LargeIcon LargeIcon { get; set; }
        [XmlElement(ElementName = "memory")]
        public string Memory { get; set; }
        [XmlElement(ElementName = "memory_policy")]
        public MemoryPolicy MemoryPolicy { get; set; }
        [XmlElement(ElementName = "migration")]
        public Migration Migration { get; set; }
        [XmlElement(ElementName = "migration_downtime")]
        public string MigrationDowntime { get; set; }
        [XmlElement(ElementName = "multi_queues_enabled")]
        public string MultiQueuesEnabled { get; set; }
        [XmlElement(ElementName = "origin")]
        public string Origin { get; set; }
        [XmlElement(ElementName = "os")]
        public Os Os { get; set; }
        [XmlElement(ElementName = "placement_policy")]
        public PlacementPolicy PlacementPolicy { get; set; }
        [XmlElement(ElementName = "small_icon")]
        public SmallIcon SmallIcon { get; set; }
        [XmlElement(ElementName = "sso")]
        public Sso Sso { get; set; }
        [XmlElement(ElementName = "start_paused")]
        public string StartPaused { get; set; }
        [XmlElement(ElementName = "stateless")]
        public string Stateless { get; set; }
        [XmlElement(ElementName = "storage_error_resume_behaviour")]
        public string StorageErrorResumeBehaviour { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "usb")]
        public Usb Usb { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "time_zone")]
        public TimeZone TimeZone { get; set; }
        [XmlElement(ElementName = "cluster")]
        public Cluster Cluster { get; set; }
        [XmlElement(ElementName = "cpu_profile")]
        public CpuProfile CpuProfile { get; set; }
        [XmlElement(ElementName = "initialization")]
        public Initialization Initialization { get; set; }
        [XmlElement(ElementName = "domain")]
        public Domain Domain { get; set; }
    }


    [XmlRoot(ElementName = "templates")]
    public class Templates
    {
        [XmlElement(ElementName = "template")]
        public List<Template> Template { get; set; }
    }

}
