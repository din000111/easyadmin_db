using EasyAdmin.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAdmin.Shared.Interfaces
{
    public interface IVm
    {
        string Id { get; set; }
        int AdapterId { get; set; }
        Adapter Adapter { get; set; }
        string Name { get; set; }
        string Status { get; set; }
        string Description { get; set; }
        string ClusterId { get; set; }        
        Cluster Cluster { get; set; }
        string ClusterName { get; set; }
        string DatacenterId { get; set; }
        string DatacenterName { get; set; }
        string PoolShortName { get; set; }        
        int MemoryGb { get; set; }
        int Cpu { get; set; }
        int HddSize { get; set; }
        string Project { get; set; }
        string Services { get; set; }
        string Domain { get; set; }
        string Manager { get; set; }
        string Admin { get; set; }
        string Owner { get; set; }
        Network Network { get; set; }
        Template Template { get; set; }
        string FullName { get; }//=> $"{Cluster?.ShortName}-{PoolShortName}-{Name}";
        string AuditDate { get; set; }
        string Deadline { get; set; }
        Cost Cost { get; set; }
        DateTime LastTimeUpdated { get; set; }
    }
}
