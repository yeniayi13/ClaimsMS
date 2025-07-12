using ClaimsMS.Application.Products.Validators;
using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Common.Dtos.Resolution.Request;
using ClaimsMS.Domain.Entities.Claim.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Validator
{
    public class CreateResolutionValidator : ValidatorBase<CreateResolutionDto>
    {
        public CreateResolutionValidator()
        {
            RuleFor(x => x.ResolutionDescription)
                .NotEmpty().WithMessage("La descripción de la resolucion es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            

        }

        
    }
}
