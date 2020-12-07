using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAdmin.Shared.Common
{
    public class Billing
    {
        public virtual Cost Cost { get; set; }
        public virtual List<Vm> Vms { get; set; }

    }
}
