using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public class ClaimAuctionId
    {
        private ClaimAuctionId(Guid value) => Value = value;

        public static ClaimAuctionId Create()
        {
            return new ClaimAuctionId(Guid.NewGuid());
        }
        public static ClaimAuctionId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new ClaimAuctionId(value);
        }

        public static ClaimAuctionId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
