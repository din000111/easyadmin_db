using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAdmin.Shared.Common
{

    public class AuditActionTypes
    {
        public enum EnumAuditActionTypes { Archive = 0, Shutdown = 1, Notify = 2 }
        public static Dictionary<EnumAuditActionTypes, string> AuditActions
        {
            get => new Dictionary<EnumAuditActionTypes, string>()
                {
                    { EnumAuditActionTypes.Archive, "Архивировать" },
                    { EnumAuditActionTypes.Notify, "Уведомить" },
                    { EnumAuditActionTypes.Shutdown, "Выключить" }
                };
            set { }
        }
    }
}
