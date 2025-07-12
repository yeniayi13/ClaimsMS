using ClaimsMS.Domain.Entities.Claim.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Resolution.ValueObject
{
    public class ResolutionId
    {
        private ResolutionId(Guid value) => Value = value;

        public static ResolutionId Create()
        {
            return new ResolutionId(Guid.NewGuid());
        }
        public static ResolutionId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ResolutionId(value);
        }

        public static ResolutionId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
