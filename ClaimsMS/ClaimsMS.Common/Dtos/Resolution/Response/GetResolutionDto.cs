using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Common.Dtos.Resolution.Response
{
    public class GetResolutionDto
    {
        public Guid ResolutionId { get; set; }
        public Guid ClaimId { get; set; }
        public string ResolutionDescription { get; set; }

        public Guid ClaimDelivery { get; set; }
    }
}
