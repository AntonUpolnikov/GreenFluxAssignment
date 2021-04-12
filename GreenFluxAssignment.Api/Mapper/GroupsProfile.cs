using AutoMapper;
using GreenFluxAssignment.Api.Contracts.Responses;

namespace GreenFluxAssignment.Api.Mapper
{
    public class GroupsProfile : Profile
    {
        public GroupsProfile()
        {
            CreateMap<Domain.Entities.Connector, Connector>()
                .ForMember(c => c.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(c => c.MaxCurrent, opt => opt.MapFrom(d => d.MaxCurrent))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();


            CreateMap<Domain.Entities.ChargeStation, ChargeStation>()
                    .ForMember(cs => cs.Id, opt => opt.MapFrom(d => d.Id))
                    .ForMember(cs => cs.Name, opt => opt.MapFrom(d => d.Name))
                    .ForMember(cs => cs.Connectors, opt => opt.MapFrom(d => d.Connectors.Values))
                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<Domain.Entities.Group, Group>()
                .ForMember(g => g.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(g => g.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(g => g.Capacity, opt => opt.MapFrom(d => d.Capacity))
                .ForMember(g => g.ChargeStations, opt => opt.MapFrom(d => d.Stations.Values))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
