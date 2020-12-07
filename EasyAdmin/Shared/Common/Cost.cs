using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EasyAdmin.Shared.Common
{
    public class Cost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AdapterId { get; set; }
        public virtual Adapter Adapter { get; set; }
        [Required]
        public string DatacenterId { get; set; }
        public string DatacenterName { get; set; }
        //[NotMapped]
        //public Datacenter Datacenter { get; set; }
        public double CpuCost { get; set; }
        public double MemoryCost { get; set; }
        public double HddCost { get; set; }
    }
}
