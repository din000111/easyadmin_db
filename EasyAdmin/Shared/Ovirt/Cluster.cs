using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;

namespace EasyAdmin.Shared.Ovirt
{
    [XmlRoot(ElementName = "cluster")]
    public class Cluster
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
        [XmlElement(ElementName = "ballooning_enabled")]
        public string BallooningEnabled { get; set; }
        [XmlElement(ElementName = "cpu")]
        public Cpu Cpu { get; set; }
        [XmlElement(ElementName = "custom_scheduling_policy_properties")]
        public CustomSchedulingPolicyProperties CustomSchedulingPolicyProperties { get; set; }
        [XmlElement(ElementName = "error_handling")]
        public ErrorHandling ErrorHandling { get; set; }
        [XmlElement(ElementName = "fencing_policy")]
        public FencingPolicy FencingPolicy { get; set; }
        [XmlElement(ElementName = "firewall_type")]
        public string FirewallType { get; set; }
        [XmlElement(ElementName = "gluster_service")]
        public string GlusterService { get; set; }
        [XmlElement(ElementName = "ha_reservation")]
        public string HaReservation { get; set; }
        [XmlElement(ElementName = "ksm")]
        public Ksm Ksm { get; set; }
        [XmlElement(ElementName = "log_max_memory_used_threshold")]
        public string LogMaxMemoryUsedThreshold { get; set; }
        [XmlElement(ElementName = "log_max_memory_used_threshold_type")]
        public string LogMaxMemoryUsedThresholdType { get; set; }
        [XmlElement(ElementName = "memory_policy")]
        public MemoryPolicy MemoryPolicy { get; set; }
        [XmlElement(ElementName = "migration")]
        public Migration Migration { get; set; }
        [XmlElement(ElementName = "required_rng_sources")]
        public RequiredRngSources RequiredRngSources { get; set; }
        [XmlElement(ElementName = "switch_type")]
        public string SwitchType { get; set; }
        [XmlElement(ElementName = "threads_as_cores")]
        public string ThreadsAsCores { get; set; }
        [XmlElement(ElementName = "trusted_service")]
        public string TrustedService { get; set; }
        [XmlElement(ElementName = "tunnel_migration")]
        public string TunnelMigration { get; set; }
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
        [XmlElement(ElementName = "virt_service")]
        public string VirtService { get; set; }
        [XmlElement(ElementName = "data_center")]
        public DataCenter DataCenter { get; set; }
        [XmlElement(ElementName = "mac_pool")]
        public MacPool MacPool { get; set; }
        [XmlElement(ElementName = "scheduling_policy")]
        public Scheduling_policy SchedulingPolicy { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "clusters")]
    public class Clusters
    {
        [XmlElement(ElementName = "cluster")]
        public List<Cluster> Cluster { get; set; }
    }
}
