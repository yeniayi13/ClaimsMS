using AutoMapper;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Domain.Entities.Resolution;
using ClaimsMS.Domain.Entities.Resolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Common.AutoMapper
{
    public class ResolutionProfile:Profile
    {
        public ResolutionProfile()
        {
            CreateMap<ResolutionEntity,GetResolutionDto>()
                .ForMember(dest => dest.ResolutionId, opt => opt.MapFrom(src => src.ResolutionId.Value))
                .ForMember(dest => dest.ClaimId, opt => opt.MapFrom(src => src.ClaimId.Value))
                .ForMember(dest => dest.ResolutionDescription, opt => opt.MapFrom(src => src.ResolutionDescription.Value));
           
        }
    }
}
