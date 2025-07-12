using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClaimsMS.Common.Dtos.Resolution.Request
{
    public class CreateResolutionDto
    {
        [JsonIgnore]
        public Guid? ResolutionId { get;  set; }
        public Guid ClaimId { get;  set; }
        public string ResolutionDescription { get;  set; }
    }
}
