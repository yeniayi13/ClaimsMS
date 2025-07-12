using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Resolution.ValueObject
{
    public class ResolutionDescription
    {
        private ResolutionDescription(string value) => Value = value;

        public static ResolutionDescription Create(string value)
        {
            try
            {
                // if (string.IsNullOrEmpty(value)) throw new NullAttributeException("Product description is required");

                return new ResolutionDescription(value);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string Value { get; init; }
    }
}
