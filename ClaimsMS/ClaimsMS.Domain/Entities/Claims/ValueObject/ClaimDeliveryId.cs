using ClaimsMS.Domain.Entities.Claim.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claims.ValueObject
{
    public class ClaimDeliveryId
    {
        private ClaimDeliveryId(Guid value) => Value = value;

        public static ClaimDeliveryId Create()
        {
            return new ClaimDeliveryId(Guid.NewGuid());
        }
        public static ClaimDeliveryId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ClaimDeliveryId(value);
        }

        public static ClaimDeliveryId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
