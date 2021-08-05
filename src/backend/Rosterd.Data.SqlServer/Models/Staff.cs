// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Rosterd.Data.SqlServer.Models
{
    public partial class Staff
    {
        public Staff()
        {
            JobStaffs = new HashSet<JobStaff>();
            StaffFacilities = new HashSet<StaffFacility>();
            StaffSkills = new HashSet<StaffSkill>();
        }

        [Key]
        public long StaffId { get; set; }

        [Required]
        public string Auth0Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string FirstName { get; set; }
        [StringLength(1000)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(1000)]
        public string LastName { get; set; }
        [StringLength(1000)]
        public string JobTitle { get; set; }
        public bool IsAvailable { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string HomePhoneNumber { get; set; }
        [StringLength(1000)]
        public string MobilePhoneNumber { get; set; }
        [StringLength(1000)]
        public string OtherPhoneNumber { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(1000)]
        public string Comments { get; set; }

        [InverseProperty(nameof(JobStaff.Staff))]
        public virtual ICollection<JobStaff> JobStaffs { get; set; }
        [InverseProperty(nameof(StaffFacility.Staff))]
        public virtual ICollection<StaffFacility> StaffFacilities { get; set; }
        [InverseProperty(nameof(StaffSkill.Staff))]
        public virtual ICollection<StaffSkill> StaffSkills { get; set; }
    }
}
