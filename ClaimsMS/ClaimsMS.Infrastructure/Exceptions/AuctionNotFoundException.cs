using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Exception
{
    public  class AuctionNotFoundException : System.Exception
    {
        public AuctionNotFoundException() { }

        public AuctionNotFoundException(string message)
            : base(message) { }

        public AuctionNotFoundException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
