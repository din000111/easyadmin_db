//using System.Collections.Generic;
//using EasyAdmin.Shared.Ovirt.Action;

//namespace EasyAdmin.Shared.Ovirt.Host
//{
//    public class Certificate
//    {
//        public string organization { get; set; }
//        public string subject { get; set; }
//    }

//    public class Topology
//    {
//        public string cores { get; set; }
//        public string sockets { get; set; }
//        public string threads { get; set; }
//    }

//    public class Cpu
//    {
//        public string name { get; set; }
//        public int speed { get; set; }
//        public Topology topology { get; set; }
//        public string type { get; set; }
//    }

//    public class DevicePassthrough
//    {
//        public string enabled { get; set; }
//    }

//    public class SupportedRngSources
//    {
//        public List<string> supported_rng_source { get; set; }
//    }

//    public class HardwareInformation
//    {
//        public string family { get; set; }
//        public string manufacturer { get; set; }
//        public string product_name { get; set; }
//        public string serial_number { get; set; }
//        public SupportedRngSources supported_rng_sources { get; set; }
//        public string uuid { get; set; }
//    }

//    public class Iscsi
//    {
//        public string initiator { get; set; }
//    }

//    public class Ksm
//    {
//        public string enabled { get; set; }
//    }

//    public class LibvirtVersion
//    {
//        public string build { get; set; }
//        public string full_version { get; set; }
//        public string major { get; set; }
//        public string minor { get; set; }
//        public string revision { get; set; }
//    }

//    public class Version
//    {
//        public string full_version { get; set; }
//        public string major { get; set; }
//    }

//    public class Os
//    {
//        public string custom_kernel_cmdline { get; set; }
//        public string reported_kernel_cmdline { get; set; }
//        public string type { get; set; }
//        public Version version { get; set; }
//    }

//    public class PmProxy
//    {
//        public string type { get; set; }
//    }

//    public class PmProxies
//    {
//        public List<PmProxy> pm_proxy { get; set; }
//    }

//    public class PowerManagement
//    {
//        public string automatic_pm_enabled { get; set; }
//        public string enabled { get; set; }
//        public string kdump_detection { get; set; }
//        public PmProxies pm_proxies { get; set; }
//    }

//    public class SeLinux
//    {
//        public string mode { get; set; }
//    }

//    public class Spm
//    {
//        public string priority { get; set; }
//        public string status { get; set; }
//    }

//    public class Ssh
//    {
//        public string fingerprint { get; set; }
//        public string port { get; set; }
//    }

//    public class Summary
//    {
//        public string active { get; set; }
//        public string migrating { get; set; }
//        public string total { get; set; }
//    }

//    public class TransparentHugepages
//    {
//        public string enabled { get; set; }
//    }

//    public class Version2
//    {
//        public string build { get; set; }
//        public string full_version { get; set; }
//        public string major { get; set; }
//        public string minor { get; set; }
//        public string revision { get; set; }
//    }

//    public class Cluster
//    {
//        public string href { get; set; }
//        public string id { get; set; }
//    }


//    public class Link2
//    {
//        public string href { get; set; }
//        public string rel { get; set; }
//    }

//    public class Host
//    {
//        public string address { get; set; }
//        public string auto_numa_status { get; set; }
//        public Certificate certificate { get; set; }
//        public Cpu cpu { get; set; }
//        public DevicePassthrough device_passthrough { get; set; }
//        public string external_status { get; set; }
//        public HardwareInformation hardware_information { get; set; }
//        public Iscsi iscsi { get; set; }
//        public string kdump_status { get; set; }
//        public Ksm ksm { get; set; }
//        public LibvirtVersion libvirt_version { get; set; }
//        public string max_scheduling_memory { get; set; }
//        public string memory { get; set; }
//        public string numa_supported { get; set; }
//        public Os os { get; set; }
//        public string port { get; set; }
//        public PowerManagement power_management { get; set; }
//        public string protocol { get; set; }
//        public SeLinux se_linux { get; set; }
//        public Spm spm { get; set; }
//        public Ssh ssh { get; set; }
//        public string status { get; set; }
//        public Summary summary { get; set; }
//        public TransparentHugepages transparent_hugepages { get; set; }
//        public string type { get; set; }
//        public string update_available { get; set; }
//        public Version2 version { get; set; }
//        public string vgpu_placement { get; set; }
//        public Cluster cluster { get; set; }
//        public Actions actions { get; set; }
//        public string name { get; set; }
//        public string comment { get; set; }
//        public string href { get; set; }
//        public string id { get; set; }
//        public List<Link2> link { get; set; }
//    }
//}
