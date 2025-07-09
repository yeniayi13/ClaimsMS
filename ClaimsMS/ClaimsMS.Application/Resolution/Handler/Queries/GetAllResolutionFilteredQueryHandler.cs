using AutoMapper;
using MediatR;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Repositories;
using ClaimsMS.Infrastructure.Exceptions;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Application.Resolution.Queries;
using ClaimsMS.Core.Repositories.Resolutions;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Infrastructure.Repositories.Resolution;


namespace ClaimsMS.Application.Resolution.Handler.Queries
{
    public class GetAllResolutionFilteredQueryHandler : IRequestHandler<GetAllResolutionFilteredQuery, List<GetResolutionDto>>
    {
        private readonly IResolutionRepositoryMongo _resolutionRepository;
        private readonly IMapper _mapper;

        public GetAllResolutionFilteredQueryHandler(IResolutionRepositoryMongo resolutionRepository, IMapper mapper)
        {
            _resolutionRepository = resolutionRepository;
            _mapper = mapper;
        }

        public async Task<List<GetResolutionDto>> Handle(GetAllResolutionFilteredQuery request, CancellationToken cancellationToken)
        {

            if (request.ClaimId == Guid.Empty)
            {
                throw new ArgumentException("El ID del reclamo es requerido para filtrar los reclamos.");
            }


            var resolution = await _resolutionRepository.GetAllByFiltredResolutionAsync(
                request.ClaimId,
                request.AuctionId,
                request.ResolutionId

            );


            if (resolution == null || !resolution.Any())
            {
                return new List<GetResolutionDto>();
            }

            var resolutionDtoList = _mapper.Map<List<GetResolutionDto>>(resolution);

            return resolutionDtoList;
        }
    }
}
