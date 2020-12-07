using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;
using StorageDomains1 = EasyAdmin.Shared.Ovirt.StorageDomains.StorageDomains;

namespace EasyAdmin.Shared.Ovirt.Disk
{
    
    [XmlRoot(ElementName = "disk_profile")]
    public class Disk_profile
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "storage_domain")]
    public class StorageDomain
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "storage_domains")]
    public class StorageDomains
    {
        [XmlElement(ElementName = "storage_domain")]
        public StorageDomain StorageDomain { get; set; }
    }

    [XmlRoot(ElementName = "disk")]
    public class Disk
    {
        [XmlElement(ElementName = "actions")]
        public Actions Actions { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "link")]
        public List<Link> Link { get; set; }
        [XmlElement(ElementName = "actual_size")]
        public string Actual_size { get; set; }
        [XmlElement(ElementName = "alias")]
        public string Alias { get; set; }
        [XmlElement(ElementName = "backup")]
        public string Backup { get; set; }
        [XmlElement(ElementName = "content_type")]
        public string Content_type { get; set; }
        [XmlElement(ElementName = "format")]
        public string Format { get; set; }
        [XmlElement(ElementName = "image_id")]
        public string Image_id { get; set; }
        [XmlElement(ElementName = "propagate_errors")]
        public string Propagate_errors { get; set; }
        [XmlElement(ElementName = "provisioned_size", IsNullable = true)]
        public double? ProvisionedSize { get; set; }
        [XmlElement(ElementName = "shareable")]
        public string Shareable { get; set; }
        [XmlElement(ElementName = "sparse")]
        public string Sparse { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "storage_type")]
        public string Storage_type { get; set; }
        [XmlElement(ElementName = "wipe_after_delete")]
        public string Wipe_after_delete { get; set; }
        [XmlElement(ElementName = "disk_profile")]
        public Disk_profile Disk_profile { get; set; }
        [XmlElement(ElementName = "storage_domains")]
        public StorageDomains StorageDomains { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "quota")]
        public Quota Quota { get; set; }
        [XmlElement(ElementName = "qcow_version")]
        public string Qcow_version { get; set; }
        [XmlElement(ElementName = "total_size")]
        public string Total_size { get; set; }
    }

    [XmlRoot(ElementName = "quota")]
    public class Quota
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "disks")]
    public class Disks
    {
        [XmlElement(ElementName = "disk")]
        public List<Disk> Disk { get; set; }
    }
}
