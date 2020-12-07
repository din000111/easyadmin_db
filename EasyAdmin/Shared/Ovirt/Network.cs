using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;

namespace EasyAdmin.Shared.Ovirt.Network
{

    [XmlRoot(ElementName = "usages")]
    public class Usages
    {
        [XmlElement(ElementName = "usage")]
        public string Usage { get; set; }
    }

    [XmlRoot(ElementName = "data_center")]
    public class Data_center
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "network")]
    public class Network
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }
        [XmlElement(ElementName = "link")]
        public List<Link> Link { get; set; }
        [XmlElement(ElementName = "mtu")]
        public string Mtu { get; set; }
        [XmlElement(ElementName = "stp")]
        public string Stp { get; set; }
        [XmlElement(ElementName = "usages")]
        public Usages Usages { get; set; }
        [XmlElement(ElementName = "data_center")]
        public Data_center Data_center { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "vlan")]
        public Vlan Vlan { get; set; }
    }

    [XmlRoot(ElementName = "vlan")]
    public class Vlan
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "networks")]
    public class Networks
    {
        [XmlElement(ElementName = "network")]
        public List<Network> Network { get; set; }
    }
}
