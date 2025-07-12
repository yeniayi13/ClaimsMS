using AutoMapper;
using ClaimsMS.Application.Claim.Queries;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Handler.Queries
{
    public class GetAllClaimQueryHandler : IRequestHandler<GetAllClaimQuery, List<GetClaimDto>>
    {
        public IClaimRepositoryMongo _claimRepository;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllClaimQueryHandler(IClaimRepositoryMongo claimRepository, IApplicationDbContext dbContext, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _dbContext = dbContext;
            _mapper = mapper; // Inyectar el Mapper
        }

        public async Task<List<GetClaimDto>> Handle(GetAllClaimQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _claimRepository.GetAllClaim();
                var claimDto = _mapper.Map<List<GetClaimDto>>(claim);
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
