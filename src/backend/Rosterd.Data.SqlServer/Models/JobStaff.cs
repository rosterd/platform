﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Rosterd.Data.SqlServer.Models
{
    [Table("JobStaff")]
    [Index(nameof(JobId), Name = "IX_JobId")]
    [Index(nameof(StaffId), Name = "IX_StaffId")]
    public partial class JobStaff
    {
        [Key]
        public long JobStaffId { get; set; }
        public long JobId { get; set; }
        public long StaffId { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }

        [ForeignKey(nameof(JobId))]
        [InverseProperty("JobStaffs")]
        public virtual Job Job { get; set; }
        [ForeignKey(nameof(StaffId))]
        [InverseProperty("JobStaffs")]
        public virtual Staff Staff { get; set; }
    }
}