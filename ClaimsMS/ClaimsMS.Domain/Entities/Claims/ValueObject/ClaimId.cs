using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public class ClaimId
    {
        private ClaimId(Guid value) => Value = value;

        public static ClaimId Create()
        {
            return new ClaimId(Guid.NewGuid());
        }
        public static ClaimId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ClaimId(value);
        }

        public static ClaimId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
