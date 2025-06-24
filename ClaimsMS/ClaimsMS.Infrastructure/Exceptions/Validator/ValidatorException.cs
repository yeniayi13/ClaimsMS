using System.Diagnostics.CodeAnalysis;

namespace ClaimsMS.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]

    public class ValidatorException : System.Exception
    {
        public ValidatorException(List<global::FluentValidation.Results.ValidationFailure> errors)
        {
            Errors = errors;
        }

        public ValidatorException(string message) : base(message)
        {
        }

        public ValidatorException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        public List<global::FluentValidation.Results.ValidationFailure> Errors { get; }
    }
}
