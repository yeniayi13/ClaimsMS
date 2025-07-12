using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Exceptions
{
    public class ClaimNotFoundException : System.Exception
    {
        public ClaimNotFoundException() { }

        public ClaimNotFoundException(string message)
            : base(message) { }

        public ClaimNotFoundException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
