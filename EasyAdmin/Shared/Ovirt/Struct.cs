using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt
{
    [XmlRoot(ElementName = "boot_menu")]
    public class BootMenu
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "bios")]
    public class Bios
    {
        [XmlElement(ElementName = "boot_menu")]
        public BootMenu BootMenu { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "migration")]
    public partial class Migration
    {
        [XmlElement(ElementName = "auto_converge")]
        public string AutoConverge { get; set; }
        [XmlElement(ElementName = "compressed")]
        public string Compressed { get; set; }
    }



    [XmlRoot(ElementName = "usb")]
    public class Usb
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "domain")]
    public class Domain
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "display")]
    public class Display
    {
        [XmlElement(ElementName = "allow_override")]
        public string AllowOverride { get; set; }
        [XmlElement(ElementName = "copy_paste_enabled")]
        public string CopyPasteEnabled { get; set; }
        [XmlElement(ElementName = "disconnect_action")]
        public string DisconnectAction { get; set; }
        [XmlElement(ElementName = "file_transfer_enabled")]
        public string FileTransferEnabled { get; set; }
        [XmlElement(ElementName = "monitors")]
        public string Monitors { get; set; }
        [XmlElement(ElementName = "single_qxl_pci")]
        public string SingleQxlPci { get; set; }
        [XmlElement(ElementName = "smartcard_enabled")]
        public string SmartcardEnabled { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "address")]
        public string Address { get; set; }
        [XmlElement(ElementName = "certificate")]
        public Certificate Certificate { get; set; }
        [XmlElement(ElementName = "port")]
        public string Port { get; set; }
        [XmlElement(ElementName = "secure_port")]
        public string SecurePort { get; set; }
    }
    [XmlRoot(ElementName = "fault")]
    public class Fault
    {
        [XmlElement(ElementName = "detail")]
        public string Detail { get; set; }
        [XmlElement(ElementName = "reason")]
        public string Reason { get; set; }
    }
    [XmlRoot(ElementName = "high_availability")]
    public partial class HighAvailability
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "priority")]
        public string Priority { get; set; }
    }

    [XmlRoot(ElementName = "io")]
    public class Io
    {
        [XmlElement(ElementName = "threads")]
        public string Threads { get; set; }
    }

    [XmlRoot(ElementName = "large_icon")]
    public partial class LargeIcon
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "memory_policy")]
    public partial class MemoryPolicy
    {
        [XmlElement(ElementName = "guaranteed")]
        public string Guaranteed { get; set; }
        [XmlElement(ElementName = "max")]
        public double? Max { get; set; }
    }

    //[XmlRoot(ElementName = "migration")]
    //public class Migration
    //{
    //    [XmlElement(ElementName = "auto_converge")]
    //    public string AutoConverge { get; set; }
    //    [XmlElement(ElementName = "compressed")]
    //    public string Compressed { get; set; }
    //}

    [XmlRoot(ElementName = "devices")]
    public class Devices
    {
        [XmlElement(ElementName = "device")]
        public List<string> Device { get; set; }
    }

    [XmlRoot(ElementName = "boot")]
    public class Boot
    {
        [XmlElement(ElementName = "devices")]
        public Devices Devices { get; set; }
    }

    [XmlRoot(ElementName = "os")]
    public class Os
    {
        [XmlElement(ElementName = "boot")]
        public Boot Boot { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "placement_policy")]
    public class PlacementPolicy
    {
        [XmlElement(ElementName = "affinity")]
        public string Affinity { get; set; }
        [XmlElement(ElementName = "hosts")]
        public Hosts Hosts { get; set; }
    }
    [XmlRoot(ElementName = "hosts")]
    public class Hosts
    {
        [XmlElement(ElementName = "host")]
        public Host Host { get; set; }
    }

    [XmlRoot(ElementName = "small_icon")]
    public class SmallIcon
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "method")]
    public class Method
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "methods")]
    public class Methods
    {
        [XmlElement(ElementName = "method")]
        public Method Method { get; set; }
    }

    [XmlRoot(ElementName = "sso")]
    public class Sso
    {
        [XmlElement(ElementName = "methods")]
        public Methods Methods { get; set; }
    }

    [XmlRoot(ElementName = "time_zone")]
    public class TimeZone
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }
    [XmlRoot(ElementName = "cpu_profile")]
    public partial class CpuProfile
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "certificate")]
    public class Certificate
    {
        [XmlElement(ElementName = "content")]
        public string Content { get; set; }
        [XmlElement(ElementName = "organization")]
        public string Organization { get; set; }
        [XmlElement(ElementName = "subject")]
        public string Subject { get; set; }
    }
    [XmlRoot(ElementName = "nic_configurations")]
    public class NicConfigurations
    {
        [XmlElement(ElementName = "nic_configuration")]
        public NicConfiguration NicConfiguration { get; set; }
    }
    [XmlRoot(ElementName = "ip")]
    public class Ip
    {
        [XmlElement(ElementName = "address")]
        public string Address { get; set; }
        [XmlElement(ElementName = "gateway")]
        public string Gateway { get; set; }
        [XmlElement(ElementName = "netmask")]
        public string Netmask { get; set; }
    }
    [XmlRoot(ElementName = "nic_configuration")]
    public class NicConfiguration
    {
        [XmlElement(ElementName = "boot_protocol")]
        public string BootProtocol { get; set; }
        [XmlElement(ElementName = "ip")]
        public Ip Ip { get; set; }
        [XmlElement(ElementName = "ipv6")]
        public string Ipv6 { get; set; }
        [XmlElement(ElementName = "ipv6_boot_protocol")]
        public string Ipv6BootProtocol { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "on_boot")]
        public string OnBoot { get; set; }
    }
    [XmlRoot(ElementName = "initialization")]
    public class Initialization
    {
        [XmlElement(ElementName = "active_directory_ou")]
        public string ActiveDirectoryOu { get; set; }
        [XmlElement(ElementName = "authorized_ssh_keys")]
        public string AuthorizedSshKeys { get; set; }
        [XmlElement(ElementName = "cloud_init_network_protocol")]
        public string CloudInitNetworkProtocol { get; set; }
        [XmlElement(ElementName = "custom_script")]
        public string CustomScript { get; set; }
        [XmlElement(ElementName = "domain")]
        public string Domain { get; set; }
        [XmlElement(ElementName = "host_name")]
        public string HostName { get; set; }
        [XmlElement(ElementName = "input_locale")]
        public string InputLocale { get; set; }
        [XmlElement(ElementName = "nic_configurations")]
        public NicConfigurations NicConfigurations { get; set; }
        [XmlElement(ElementName = "org_name")]
        public string OrgName { get; set; }
        [XmlElement(ElementName = "regenerate_ssh_keys")]
        public string RegenerateSshKeys { get; set; }
        [XmlElement(ElementName = "system_locale")]
        public string SystemLocale { get; set; }
        [XmlElement(ElementName = "ui_language")]
        public string UiLanguage { get; set; }
        [XmlElement(ElementName = "user_locale")]
        public string UserLocale { get; set; }
        [XmlElement(ElementName = "user_name")]
        public string UserName { get; set; }
    }

    [XmlRoot(ElementName = "version")]
    public partial class Version
    {
        [XmlElement(ElementName = "build")]
        public string Build { get; set; }
        [XmlElement(ElementName = "full_version")]
        public string FullVersion { get; set; }
        [XmlElement(ElementName = "major")]
        public string Major { get; set; }
        [XmlElement(ElementName = "minor")]
        public string Minor { get; set; }
        [XmlElement(ElementName = "revision")]
        public string Revision { get; set; }
        [XmlElement(ElementName = "version_name")]
        public string VersionName { get; set; }
        [XmlElement(ElementName = "version_number")]
        public string VersionNumber { get; set; }
        [XmlElement(ElementName = "base_template")]
        public BaseTemplate BaseTemplate { get; set; }
    }

    [XmlRoot(ElementName = "kernel")]
    public class Kernel
    {
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
    }

    [XmlRoot(ElementName = "guest_operating_system")]
    public class GuestOperatingSystem
    {
        [XmlElement(ElementName = "architecture")]
        public string Architecture { get; set; }
        [XmlElement(ElementName = "codename")]
        public string Codename { get; set; }
        [XmlElement(ElementName = "distribution")]
        public string Distribution { get; set; }
        [XmlElement(ElementName = "family")]
        public string Family { get; set; }
        [XmlElement(ElementName = "kernel")]
        public Kernel Kernel { get; set; }
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
    }

    [XmlRoot(ElementName = "guest_time_zone")]
    public class GuestTimeZone
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "utc_offset")]
        public string UtcOffset { get; set; }
    }

    [XmlRoot(ElementName = "host")]
    public class Host
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "cpu")]
    public class Cpu
    {
        [XmlElement(ElementName = "architecture")]
        public string Architecture { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "topology")]
        public Topology Topology { get; set; }
    }
    [XmlRoot(ElementName = "topology")]
    public class Topology
    {
        [XmlElement(ElementName = "cores")]
        public int Cores { get; set; }
        [XmlElement(ElementName = "sockets")]
        public int Sockets { get; set; }
        [XmlElement(ElementName = "threads")]
        public int Threads { get; set; }
    }
    [XmlRoot(ElementName = "property")]
    public class Property
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "custom_scheduling_policy_properties")]
    public class CustomSchedulingPolicyProperties
    {
        [XmlElement(ElementName = "property")]
        public List<Property> Property { get; set; }
    }

    [XmlRoot(ElementName = "error_handling")]
    public class ErrorHandling
    {
        [XmlElement(ElementName = "on_error")]
        public string OnError { get; set; }
    }

    [XmlRoot(ElementName = "skip_if_connectivity_broken")]
    public class SkipIfConnectivityBroken
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "threshold")]
        public string Threshold { get; set; }
    }

    [XmlRoot(ElementName = "skip_if_sd_active")]
    public class SkipIfSdActive
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "fencing_policy")]
    public class FencingPolicy
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "skip_if_connectivity_broken")]
        public SkipIfConnectivityBroken Skip_if_connectivity_broken { get; set; }
        [XmlElement(ElementName = "skip_if_gluster_bricks_up")]
        public string Skip_if_gluster_bricks_up { get; set; }
        [XmlElement(ElementName = "skip_if_gluster_quorum_not_met")]
        public string Skip_if_gluster_quorum_not_met { get; set; }
        [XmlElement(ElementName = "skip_if_sd_active")]
        public SkipIfSdActive Skip_if_sd_active { get; set; }
    }

    [XmlRoot(ElementName = "ksm")]
    public class Ksm
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "merge_across_nodes")]
        public string Merge_across_nodes { get; set; }
    }

    [XmlRoot(ElementName = "over_commit")]
    public class OverCommit
    {
        [XmlElement(ElementName = "percent")]
        public string Percent { get; set; }
    }

    [XmlRoot(ElementName = "transparent_hugepages")]
    public class TransparentHugepages
    {
        [XmlElement(ElementName = "enabled")]
        public string Enabled { get; set; }
    }


    public partial class MemoryPolicy
    {
        [XmlElement(ElementName = "over_commit")]
        public OverCommit Over_commit { get; set; }
        [XmlElement(ElementName = "transparent_hugepages")]
        public TransparentHugepages Transparent_hugepages { get; set; }
    }

    [XmlRoot(ElementName = "bandwidth")]
    public class Bandwidth
    {
        [XmlElement(ElementName = "assignment_method")]
        public string Assignment_method { get; set; }
    }

    [XmlRoot(ElementName = "policy")]
    public class Policy
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    public partial class Migration
    {

        [XmlElement(ElementName = "bandwidth")]
        public Bandwidth Bandwidth { get; set; }

        [XmlElement(ElementName = "policy")]
        public Policy Policy { get; set; }
    }

    [XmlRoot(ElementName = "required_rng_sources")]
    public class RequiredRngSources
    {
        [XmlElement(ElementName = "required_rng_source")]
        public string Required_rng_source { get; set; }
    }


    [XmlRoot(ElementName = "data_center")]
    public class DataCenter
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "mac_pool")]
    public class MacPool
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "scheduling_policy")]
    public class Scheduling_policy
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
}
