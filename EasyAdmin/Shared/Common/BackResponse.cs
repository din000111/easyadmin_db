using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAdmin.Shared.Common
{
    public class BackResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
}
