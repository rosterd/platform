﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Rosterd.Data.SqlServer.Models
{
    [Table("Job")]
    [Index(nameof(JobId), Name = "Unq_Job_JobId", IsUnique = true)]
    public partial class Job
    {
        public Job()
        {
            JobSkills = new HashSet<JobSkill>();
            JobStatusChanges = new HashSet<JobStatusChange>();
        }

        [Key]
        public long JobId { get; set; }
        [Required]
        [StringLength(1000)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(8000)]
        public string Description { get; set; }
        public long FacilityId { get; set; }
        [Column("JobStartDateTimeUTC")]
        public DateTime JobStartDateTimeUtc { get; set; }
        [Column("JobEndDateTimeUTC")]
        public DateTime JobEndDateTimeUtc { get; set; }
        [StringLength(1000)]
        public string Comments { get; set; }
        public long? GracePeriodToCancelMinutes { get; set; }
        public bool? NoGracePeriod { get; set; }

        [InverseProperty(nameof(JobSkill.Job))]
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        [InverseProperty(nameof(JobStatusChange.Job))]
        public virtual ICollection<JobStatusChange> JobStatusChanges { get; set; }
    }
}