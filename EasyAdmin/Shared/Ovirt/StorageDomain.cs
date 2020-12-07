using EasyAdmin.Shared.Ovirt.Action;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt.StorageDomains
{

    [XmlRoot(ElementName = "storage")]
    public class Storage
    {
        [XmlElement(ElementName = "address")]
        public string Address { get; set; }
        [XmlElement(ElementName = "nfs_version")]
        public string Nfs_version { get; set; }
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "volume_group")]
        public VolumeGroup Volume_group { get; set; }
        [XmlElement(ElementName = "vfs_type")]
        public string Vfs_type { get; set; }
        [XmlElement(ElementName = "mount_options")]
        public string Mount_options { get; set; }
    }

    [XmlRoot(ElementName = "data_center")]
    public class Data_center
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "data_centers")]
    public class Data_centers
    {
        [XmlElement(ElementName = "data_center")]
        public Data_center Data_center { get; set; }
    }

    [XmlRoot(ElementName = "storage_domain")]
    public class StorageDomain
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
        [XmlElement(ElementName = "available")]
        public double Available { get; set; }
        [XmlElement(ElementName = "backup")]
        public string Backup { get; set; }
        [XmlElement(ElementName = "block_size")]
        public string Block_size { get; set; }
        [XmlElement(ElementName = "committed")]
        public string Committed { get; set; }
        [XmlElement(ElementName = "critical_space_action_blocker")]
        public string Critical_space_action_blocker { get; set; }
        [XmlElement(ElementName = "discard_after_delete")]
        public string Discard_after_delete { get; set; }
        [XmlElement(ElementName = "external_status")]
        public string External_status { get; set; }
        [XmlElement(ElementName = "master")]
        public bool Master { get; set; }
        [XmlElement(ElementName = "storage")]
        public Storage Storage { get; set; }
        [XmlElement(ElementName = "storage_format")]
        public string Storage_format { get; set; }
        [XmlElement(ElementName = "supports_discard")]
        public string Supports_discard { get; set; }
        [XmlElement(ElementName = "supports_discard_zeroes_data")]
        public string Supports_discard_zeroes_data { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "used")]
        public string Used { get; set; }
        [XmlElement(ElementName = "warning_low_space_indicator")]
        public string Warning_low_space_indicator { get; set; }
        [XmlElement(ElementName = "wipe_after_delete")]
        public string Wipe_after_delete { get; set; }
        [XmlElement(ElementName = "data_centers")]
        public Data_centers Data_centers { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
    }

    [XmlRoot(ElementName = "logical_unit")]
    public class Logical_unit
    {
        [XmlElement(ElementName = "discard_max_size")]
        public string Discard_max_size { get; set; }
        [XmlElement(ElementName = "discard_zeroes_data")]
        public string Discard_zeroes_data { get; set; }
        [XmlElement(ElementName = "lun_mapping")]
        public string Lun_mapping { get; set; }
        [XmlElement(ElementName = "paths")]
        public string Paths { get; set; }
        [XmlElement(ElementName = "product_id")]
        public string Product_id { get; set; }
        [XmlElement(ElementName = "serial")]
        public string Serial { get; set; }
        [XmlElement(ElementName = "size")]
        public string Size { get; set; }
        [XmlElement(ElementName = "storage_domain_id")]
        public string Storage_domain_id { get; set; }
        [XmlElement(ElementName = "vendor_id")]
        public string Vendor_id { get; set; }
        [XmlElement(ElementName = "volume_group_id")]
        public string Volume_group_id { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "logical_units")]
    public class Logical_units
    {
        [XmlElement(ElementName = "logical_unit")]
        public Logical_unit Logical_unit { get; set; }
    }

    [XmlRoot(ElementName = "volume_group")]
    public class VolumeGroup
    {
        [XmlElement(ElementName = "logical_units")]
        public Logical_units Logical_units { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "storage_domains")]
    public class StorageDomains
    {
        [XmlElement(ElementName = "storage_domain")]
        public List<StorageDomain> StorageDomain { get; set; }
    }
}
