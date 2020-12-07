using System.Collections.Generic;
using System.Xml.Serialization;
using EasyAdmin.Shared.Ovirt.Action;

namespace EasyAdmin.Shared.Ovirt.VNicProfile
{
    
    [XmlRoot(ElementName = "pass_through")]
    public class PassThrough
    {
        [XmlElement(ElementName = "mode")]
        public string Mode { get; set; }
    }

    [XmlRoot(ElementName = "network")]
    public class Network
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "network_filter")]
    public class NetworkFilter
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "vnic_profile")]
    public class VnicProfile
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "link")]
        public Link Link { get; set; }
        [XmlElement(ElementName = "pass_through")]
        public PassThrough PassThrough { get; set; }
        [XmlElement(ElementName = "port_mirroring")]
        public string PortMirroring { get; set; }
        [XmlElement(ElementName = "network")]
        public Network Network { get; set; }
        [XmlElement(ElementName = "network_filter")]
        public NetworkFilter NetworkFilter { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "vnic_profiles")]
    public class VnicProfiles
    {
        [XmlElement(ElementName = "vnic_profile")]
        public List<VnicProfile> VnicProfile { get; set; }
    }

    
}
