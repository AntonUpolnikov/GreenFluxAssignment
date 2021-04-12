using System;
using System.Threading.Tasks;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Domain.Interfaces.Services;

namespace GreenFluxAssignment.Domain.Services
{
    public class ConnectorService : IConnectorService
    {
        private readonly IGroupRepository _groupRepository;

        public ConnectorService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Connector> ChangeCurrent(Guid groupId, Guid stationId, int connectorId, decimal current)
        {
            Group group = await _groupRepository.GetById(groupId);
            group.ChangeConnectorCurrent(stationId, connectorId, current);
            await _groupRepository.Save(group);

            return group.Stations[stationId].Connectors[connectorId];
        }

        public async Task<Connector> Create(Guid groupId, Guid stationId, decimal maxCurrent)
        {
            Group group = await _groupRepository.GetById(groupId);
            Connector connector = group.AddConnector(stationId, maxCurrent);
            await _groupRepository.Save(group);

            return connector;
        }

        public async Task<Connector> Get(Guid groupId, Guid stationId, int connectorId)
        {
            Group group = await _groupRepository.GetById(groupId);

            return group.Stations[stationId].Connectors[connectorId];
        }

        public async Task<Connector> Remove(Guid groupId, Guid stationId, int connectorId)
        {
            Group group = await _groupRepository.GetById(groupId);
            Connector connector = group.RemoveConnector(stationId, connectorId);
            await _groupRepository.Save(group);

            return connector;
        }
    }
}
