using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public class ClaimReason
    {
        private ClaimReason(string value) => Value = value;

        public static ClaimReason Create(string value)
        {
            try
            {
                // if (string.IsNullOrEmpty(value)) throw new NullAttributeException("Product description is required");

                return new ClaimReason(value);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string Value { get; init; }
    }
}
