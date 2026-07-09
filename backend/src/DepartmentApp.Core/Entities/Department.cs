using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("Departments")]
    public class Department : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public long BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<JobPosting> JobPostings { get; set; }

        public virtual ICollection<TrainingPlan> TrainingPlans { get; set; }

    }
}