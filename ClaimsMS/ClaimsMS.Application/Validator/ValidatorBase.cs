using ClaimsMS.Infrastructure.Exceptions;
using FluentValidation;

namespace ClaimsMS.Application.Products.Validators
{
    public class ValidatorBase<T> : AbstractValidator<T>
    {
        public virtual async Task<bool> ValidateRequest(T request)
        {
            var result = await ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidatorException(result.Errors.ToString());
            }

            return result.IsValid;
        }
    }
}
