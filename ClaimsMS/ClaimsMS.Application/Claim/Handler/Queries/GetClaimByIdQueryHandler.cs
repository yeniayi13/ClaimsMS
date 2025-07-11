using AutoMapper;
using MediatR;
using ClaimsMS.Core.Database;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Core.Repositories;
using ClaimsMS.Domain.Entities.Claim;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Infrastructure.Exceptions;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Core.Repositories.Claims;


namespace ClaimsMS.Application.Claim.Handler.Queries
{
 
        public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, GetClaimDto>
        {
            public IClaimRepositoryMongo _claimRepository;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetClaimByIdQueryHandler(IClaimRepositoryMongo claimRepository, IApplicationDbContext dbContext, IMapper mapper)
            {
                 _claimRepository = claimRepository;
                _dbContext = dbContext;
                _mapper = mapper; // Inyectar el Mapper
            }

            public async Task<GetClaimDto> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.ClaimId == Guid.Empty) throw new ClaimNotFoundException("Claim id is required");
                   // var claimId = ClaimId.Create(request.ClaimId);
                    var claim = await _claimRepository.GetByIdAsync((Guid)request.ClaimId);
                    var claimDto = _mapper.Map<GetClaimDto>(claim);
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
