using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EasyAdmin.Shared.Ovirt
{
	[XmlRoot(ElementName = "link")]
	public class Link
	{
		[XmlAttribute(AttributeName = "href")]
		public string Href { get; set; }
		[XmlAttribute(AttributeName = "rel")]
		public string Rel { get; set; }
	}

	[XmlRoot(ElementName = "actions")]
	public class Actions
	{
		[XmlElement(ElementName = "link")]
		public List<Link> Link { get; set; }
	}

	[XmlRoot(ElementName = "graphics_console")]
	public class GraphicsConsole
	{
		[XmlElement(ElementName = "actions")]
		public Actions Actions { get; set; }
		[XmlElement(ElementName = "protocol")]
		public string Protocol { get; set; }
		[XmlElement(ElementName = "vm")]
		public Vm Vm { get; set; }
		[XmlAttribute(AttributeName = "href")]
		public string Href { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "graphics_consoles")]
	public class GraphicsConsoles
	{
		[XmlElement(ElementName = "graphics_console")]
		public List<GraphicsConsole> GraphicsConsole { get; set; }
	}
}
