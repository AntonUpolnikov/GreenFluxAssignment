using System.Linq;
using AutoMapper;
using GreenFluxAssignment.Persistence.Entities;

namespace GreenFluxAssignment.Persistence.Mapper
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
                    .AfterMap((_, cs) =>
                    {
                        foreach (var connector in cs.Connectors)
                        {
                            connector.ChargeStationId = cs.Id;
                        }
                    })
                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<Domain.Entities.Group, Group>()
                .ForMember(g => g.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(g => g.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(g => g.Capacity, opt => opt.MapFrom(d => d.Capacity))
                .ForMember(g => g.ChargeStations, opt => opt.MapFrom(d => d.Stations.Values))
                .ForMember(g => g.Timestamp, opt => opt.Ignore())
                .AfterMap((_, g) =>
                {
                    foreach (var station in g.ChargeStations)
                    {
                        station.GroupId = g.Id;
                    }
                })
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
