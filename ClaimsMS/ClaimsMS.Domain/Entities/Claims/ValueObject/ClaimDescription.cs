using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public class ClaimDescription
    {
        private ClaimDescription(string value) => Value = value;

        public static ClaimDescription Create(string value)
        {
            try
            {
                // if (string.IsNullOrEmpty(value)) throw new NullAttributeException("Product description is required");

                return new ClaimDescription(value);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string Value { get; init; }
    }
}
