using AutoMapper;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Models;

namespace HackathonDataAnalysis.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Reading, ReadingDto>();
        CreateMap<Rule, RuleDto>()
            .ForMember(dest => dest.SensorType, opt => opt.MapFrom(src => new NameIdDto((int)src.SensorType, src.SensorType.GetDescription())))
            .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => new NameIdDto((int)src.Operator, src.Operator.GetDescription())));
        CreateMap<SensorType, NameIdDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.GetDescription()));
        CreateMap<Operator, NameIdDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.GetDescription()));
    }
}