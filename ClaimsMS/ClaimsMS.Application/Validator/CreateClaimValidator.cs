using ClaimsMS.Application.Products.Validators;
using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Domain.Entities.Claim.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Validator
{
    public class CreateClaimValidator : ValidatorBase<CreateClaimDto>
    {
        public CreateClaimValidator()
        {
            RuleFor(x => x.ClaimDescription)
                .NotEmpty().WithMessage("La descripción de la reclamación es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            RuleFor(x => x.ClaimReason)
                .NotEmpty().WithMessage("Debe especificar una razón para la reclamación.")
                .MaximumLength(300).WithMessage("La razón no puede superar los 300 caracteres.");

            RuleFor(x => x.StatusClaim)
                .NotEmpty().WithMessage("Debe asignar un estado a la reclamación.")
                .Must(BeAValidStatus).WithMessage("El estado proporcionado no es válido.");

           
        }

        private bool BeAValidStatus(string status)
        {
            return Enum.TryParse(typeof(StatusClaim), status, ignoreCase: true, out _);
        }
    }
}
