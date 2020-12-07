using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt.Nic
{

    [XmlRoot(ElementName = "vm")]
    public class Vm
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "mac")]
    public class Mac
    {
        [XmlElement(ElementName = "address")]
        public string Address { get; set; }
    }

    [XmlRoot(ElementName = "vnic_profile")]
    public class Vnic_profile
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "nic")]
    public class Nic
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "vm")]
        public Vm Vm { get; set; }
        [XmlElement(ElementName = "interface")]
        public string Interface { get; set; }
        [XmlElement(ElementName = "linked")]
        public string Linked { get; set; }
        [XmlElement(ElementName = "mac")]
        public Mac Mac { get; set; }
        [XmlElement(ElementName = "plugged")]
        public string Plugged { get; set; }
        [XmlElement(ElementName = "vnic_profile")]
        public Vnic_profile Vnic_profile { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
}
