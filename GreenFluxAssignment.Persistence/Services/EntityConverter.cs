using System.Linq;
using AutoMapper;
using GreenFluxAssignment.Persistence.Entities;
using GreenFluxAssignment.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace GreenFluxAssignment.Persistence.Services
{
    internal class EntityConverter : IEntityConverter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EntityConverter> _logger;

        public EntityConverter(IMapper mapper, ILogger<EntityConverter> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public Domain.Entities.Group FromEntity(Group entity)
        {
            var group = new Domain.Entities.Group(entity.Id, entity.Name, entity.Capacity);
            foreach (var station in entity.ChargeStations)
            {
                group.AddStation(FromEntity(station));
            }

            return group;
        }

        public Group ToEntity(Domain.Entities.Group group, Group entity = null)
        {
            return entity is null
                ? _mapper.Map<Group>(group)
                : _mapper.Map(group, entity);
        }


        private Domain.Entities.ChargeStation FromEntity(ChargeStation entity)
        {
            var connectors = entity.Connectors
                .Select(c => new Domain.Entities.Connector(c.Id, c.MaxCurrent));

            return new Domain.Entities.ChargeStation(entity.Id, entity.Name, connectors);
        }
    }
}
