using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public class ClaimProductId
    {
        private ClaimProductId(Guid value) => Value = value;

        public static ClaimProductId Create()
        {
            return new ClaimProductId(Guid.NewGuid());
        }
        public static ClaimProductId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ClaimProductId(value);
        }

        public static ClaimProductId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }

    }
}
