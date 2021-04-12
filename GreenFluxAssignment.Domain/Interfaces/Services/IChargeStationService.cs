using GreenFluxAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Domain.Interfaces.Services
{
    public interface IChargeStationService
    {
        Task<ChargeStation> Create(Guid groupId, string name, decimal maxCurrent);
        Task<ChargeStation> Remove(Guid groupId, Guid stationId);
        Task<ChargeStation> ChangeName(Guid groupId, Guid stationId, string name);
        Task<ChargeStation> Get(Guid groupId, Guid stationId);
    }
}
