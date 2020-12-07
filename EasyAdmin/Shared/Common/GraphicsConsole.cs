using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAdmin.Shared.Common
{
    public class GraphicsConsole
    {
        
        public GraphicsConsoleType Type { get; set; }
        public string Base64ConsoleFile { get; set; }
        public Uri ConsoleUri { get; set; }
    }

    public enum GraphicsConsoleType { File = 0, Web = 1 }
}
