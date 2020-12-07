using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt.Action
{
    [XmlRoot(ElementName = "job")]
    public class Job
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "action")]
    public class Action
    {
        [XmlElement(ElementName = "job")]
        public Job Job { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "remote_viewer_connection_file")]
        public string RemoteViewerConnectionFile { get; set; }
    }
    [XmlRoot(ElementName = "actions")]
    public class Actions
    {
        [XmlElement(ElementName = "link")]
        public List<Link> Link { get; set; }
    }

    [XmlRoot(ElementName = "link")]
    public class Link
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }
    }
}
