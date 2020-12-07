using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt.Disk
{
    [XmlRoot(ElementName = "disk_attachment")]
    public class DiskAttachment
    {
        [XmlElement(ElementName = "bootable", IsNullable = true)]
        public bool? Bootable { get; set; }
        [XmlElement(ElementName = "interface")]
        public string Interface { get; set; }
        [XmlElement(ElementName = "active", IsNullable = true)]
        public bool? Active { get; set; }
        [XmlElement(ElementName = "disk")]
        public Disk Disk { get; set; }
    }
    [XmlRoot(ElementName = "disk_attachments")]
    public class DiskAttachments
    {
        [XmlElement(ElementName = "disk_attachment")]
        public List<DiskAttachment> DiskAttachment { get; set; }
    }

}
