using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string message)
            : base(message) { }

        public UserNotFoundException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
