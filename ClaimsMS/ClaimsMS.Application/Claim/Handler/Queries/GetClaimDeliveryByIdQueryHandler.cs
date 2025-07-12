using AutoMapper;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Handler.Queries
{
    public class GetClaimDeliveryByIdQueryHandler: IRequestHandler<GetClaimDeliveryByIdQuery, GetClaimDeliveryDto>

    {
                public IClaimDeliveryRepositoryMongo _claimRepository;
                private readonly IApplicationDbContext _dbContext;
                private readonly IMapper _mapper;

        public GetClaimDeliveryByIdQueryHandler(IClaimDeliveryRepositoryMongo claimRepository, IApplicationDbContext dbContext, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _dbContext = dbContext;
            _mapper = mapper; // Inyectar el Mapper
        }

        public async Task<GetClaimDeliveryDto> Handle(GetClaimDeliveryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ClaimId == Guid.Empty) throw new ClaimNotFoundException("El id del reclamo es necesario.");
               // var claimId = ClaimId.Create(request.ClaimId);
                var claim = await _claimRepository.GetByIdAsync(request.ClaimId);
                var claimDto = _mapper.Map<GetClaimDeliveryDto>(claim);
                return claimDto;
            }
            catch (ClaimNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
