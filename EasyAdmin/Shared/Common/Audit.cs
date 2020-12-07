using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasyAdmin.Shared.Common
{

    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AuditDays { get; set; }
        public AuditActionTypes.EnumAuditActionTypes AuditActionType { get; set; }
        [NotMapped]
        public string AuditActionName => AuditActionTypes.AuditActions[AuditActionType];
        public int AdapterId { get; set; }
        public virtual Adapter Adapter { get; set; }
        public string DatacenterId { get; set; }
        public string DatacenterName { get; set; }
        public string ClusterId { get; set; }
        public string ClusterName { get; set; }

    }
}
