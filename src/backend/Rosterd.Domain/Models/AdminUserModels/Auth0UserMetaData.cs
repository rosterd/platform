using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Enums;

namespace Rosterd.Domain.Models.AdminUserModels
{
    public class Auth0UserMetaData
    {
        [Required]
        public string FacilityIdsCsvString { get; set; }
    }
}
