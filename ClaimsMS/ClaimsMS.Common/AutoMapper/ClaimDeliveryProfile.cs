using AutoMapper;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Common.AutoMapper
{
    public class ClaimDeliveryProfile : Profile
    {
        public ClaimDeliveryProfile()
        {
            CreateMap<ClaimDelivery, GetClaimDeliveryDto>()
                .ForMember(dest => dest.ClaimId, opt => opt.MapFrom(src => src.ClaimId.Value))
                .ForMember(dest => dest.ClaimDeliveryId, opt => opt.MapFrom(src => src.ClaimDeliveryId.Value))
                .ForMember(dest => dest.ClaimDescription, opt => opt.MapFrom(src => src.ClaimDescription.Value))
                .ForMember(dest => dest.ClaimReason, opt => opt.MapFrom(src => src.ClaimReason.Value))
                .ForMember(dest => dest.StatusClaim, opt => opt.MapFrom(src => src.StatusClaim.ToString()))
                .ForMember(dest => dest.ClaimEvidence, opt => opt.MapFrom(src => src.ClaimEvidence.Base64Data))
                .ForMember(dest => dest.ClaimUserId, opt => opt.MapFrom(src => src.ClaimUserId.Value))
                .ReverseMap();

        }
    }
}
