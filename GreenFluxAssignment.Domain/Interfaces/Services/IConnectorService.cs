using GreenFluxAssignment.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Domain.Interfaces.Services
{
    public interface IConnectorService
    {
        Task<Connector> Create(Guid groupId, Guid stationId, decimal maxCurrent);
        Task<Connector> Remove(Guid groupId, Guid stationId, int connectorId);
        Task<Connector> ChangeCurrent(Guid groupId, Guid stationId, int connectorId, decimal current);
        Task<Connector> Get(Guid groupId, Guid stationId, int connectorId);
    }
}
