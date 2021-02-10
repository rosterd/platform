﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Rosterd.Data.SqlServer.Models
{
    public partial class StaffSkill
    {
        [Key]
        public long StaffSkillId { get; set; }
        public long StaffId { get; set; }
        public long SkillId { get; set; }

        [ForeignKey(nameof(StaffId))]
        [InverseProperty("StaffSkill")]
        public virtual Staff Staff { get; set; }
    }
}