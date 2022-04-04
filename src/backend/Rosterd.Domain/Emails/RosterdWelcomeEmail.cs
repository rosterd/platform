using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rosterd.Domain.Emails
{
    public class RosterdWelcomeEmail
    {
        [JsonPropertyName("organization")]
        public string Organization { get; set; }

        [JsonPropertyName("name")]
        public string StaffName { get; set; }
    }
}
