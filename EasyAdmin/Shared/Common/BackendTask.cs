using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyAdmin.Shared.Common
{
    public class BackendTask
    {
        [Key]
        public int Id { get; set; }
        public string BackendTaskId { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public int AdapterId { get; set; }
        public Adapter Adapter { get; set; }
        public bool IsEnded { get; set; }
        public string Status { get; set; }
        public bool IsVisible { get; set; }
        public string UserId { get; set; }
        public string RelatedEntityType { get; set; }
        public string RelatedEntityName { get; set; }
    }
}
