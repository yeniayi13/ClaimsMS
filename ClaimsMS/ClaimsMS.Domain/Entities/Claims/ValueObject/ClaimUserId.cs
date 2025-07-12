using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claims.ValueObject
{
    public class ClaimUserId
    {
        private ClaimUserId(Guid value) => Value = value;

        public static ClaimUserId Create()
        {
            return new ClaimUserId(Guid.NewGuid());
        }
        public static ClaimUserId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ClaimUserId(value);
        }
        public Guid Value { get; init; }
    }
}
