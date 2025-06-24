using AutoMapper;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Domain.Entities.Claim;
using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Common.AutoMapper
{
    public class ClaimProfile: Profile
    {
        public ClaimProfile()
        {
            CreateMap<ClaimEntity, GetClaimDto>()
                .ForMember(dest => dest.ClaimId, opt => opt.MapFrom(src => src.ClaimId))
                .ForMember(dest => dest.ClaimAuctionId, opt => opt.MapFrom(src => src.ClaimAuctionId))
                .ForMember(dest => dest.ClaimDescription, opt => opt.MapFrom(src => src.ClaimDescription.Value))
                .ForMember(dest => dest.ClaimReason, opt => opt.MapFrom(src => src.ClaimReason.Value))
                .ForMember(dest => dest.StatusClaim, opt => opt.MapFrom(src => src.StatusClaim))
                .ForMember(dest => dest.ClaimEvidence, opt => opt.MapFrom(src => src.ClaimEvidence.Base64Data))
                .ForMember(dest => dest.ClaimUserId, opt => opt.MapFrom(src => src.ClaimUserId.Value));

        }
    }
}
