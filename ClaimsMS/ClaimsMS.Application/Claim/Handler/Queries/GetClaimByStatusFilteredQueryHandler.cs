using AutoMapper;
using MediatR;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Repositories;
using ClaimsMS.Infrastructure.Exceptions;
using ClaimsMS.Core.Repositories.Claims;


namespace ClaimsMS.Application.Claim.Handler.Queries
{

        public class GetClaimByStatusFilteredQueryHandler : IRequestHandler<GetClaimByStatusFilteredQuery, List<GetClaimDto>>
        {
            private readonly IClaimRepositoryMongo _claimRepository;
            private readonly IMapper _mapper;

            public GetClaimByStatusFilteredQueryHandler(IClaimRepositoryMongo claimRepository, IMapper mapper)
            {
            _claimRepository = claimRepository;
                _mapper = mapper;
            }

            public async Task<List<GetClaimDto>> Handle(GetClaimByStatusFilteredQuery request, CancellationToken cancellationToken)
            {

                if (request.UserId == Guid.Empty)
                {
                    throw new ArgumentException("El ID de usuario es requerido para filtrar reclamos.");
                }


                var claims = await _claimRepository.GetByStatusClaimsAsync(
                    request.UserId,
                    request.auctionId,
                    request.status
                   
                );


                if (claims == null || !claims.Any())
                {
                    return new List<GetClaimDto>();
                }

                var claimsDtoList = _mapper.Map<List<GetClaimDto>>(claims);

                return claimsDtoList;
            }
        }
    }
