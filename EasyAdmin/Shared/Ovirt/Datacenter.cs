using EasyAdmin.Shared.Ovirt.Action;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt.Datacenter
{

    [XmlRoot(ElementName = "version")]
    public class Version
    {
        [XmlElement(ElementName = "major")]
        public string Major { get; set; }
        [XmlElement(ElementName = "minor")]
        public string Minor { get; set; }
    }

    [XmlRoot(ElementName = "supported_versions")]
    public class Supported_versions
    {
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
    }

    [XmlRoot(ElementName = "mac_pool")]
    public class Mac_pool
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "data_center")]
    public class DataCenter
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "link")]
        public List<Link> Link { get; set; }
        [XmlElement(ElementName = "local")]
        public string Local { get; set; }
        [XmlElement(ElementName = "quota_mode")]
        public string Quota_mode { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "storage_format")]
        public string Storage_format { get; set; }
        [XmlElement(ElementName = "supported_versions")]
        public Supported_versions Supported_versions { get; set; }
        [XmlElement(ElementName = "version")]
        public Version Version { get; set; }
        [XmlElement(ElementName = "mac_pool")]
        public Mac_pool Mac_pool { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "data_centers")]
    public class DataCenters
    {
        [XmlElement(ElementName = "data_center")]
        public List<DataCenter> DataCenter { get; set; }
    }
}
