﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Rosterd.Data.SqlServer.Models
{
    [Table("Tenant")]
    public partial class Tenant
    {
        public Tenant()
        {
            Organizations = new HashSet<Organization>();
        }

        [Key]
        public long TenantId { get; set; }
        [Required]
        [StringLength(1000)]
        public string TenantName { get; set; }
        [StringLength(1000)]
        public string BusinessName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string WebsiteLink { get; set; }

        [InverseProperty(nameof(Organization.Tenant))]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}