using AutoMapper;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Repositories.Claims;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Handler.Queries
{
    public class GetClaimDeliveryByStatusFilteredQueryHandler : IRequestHandler<GetClaimDeliveryByStatusFilteredQuery, List<GetClaimDeliveryDto>>
    {
        private readonly IClaimDeliveryRepositoryMongo _claimRepository;
        private readonly IMapper _mapper;

        public GetClaimDeliveryByStatusFilteredQueryHandler(IClaimDeliveryRepositoryMongo claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<List<GetClaimDeliveryDto>> Handle(GetClaimDeliveryByStatusFilteredQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (request.UserId == Guid.Empty)
                {
                    throw new ArgumentException("El ID de usuario es requerido para filtrar reclamos.");
                }


                var claims = await _claimRepository.GetByStatusClaimsAsync(
                    request.UserId,
                    request.deliveryId,
                    request.status

                );


                if (claims == null || !claims.Any())
                {
                    return new List<GetClaimDeliveryDto>();
                }

                var claimsDtoList = _mapper.Map<List<GetClaimDeliveryDto>>(claims);

                return claimsDtoList;
            }
            catch(ArgumentException ex)
            {
                throw new ArgumentException("Error al filtrar reclamos por estado: " + ex.Message, ex);
            }
           
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
