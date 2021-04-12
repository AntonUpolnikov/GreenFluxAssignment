using System;
using System.Threading.Tasks;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Domain.Interfaces.Services;

namespace GreenFluxAssignment.Domain.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IGroupRepository _groupRepository;

        public ChargeStationService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<ChargeStation> ChangeName(Guid groupId, Guid stationId, string name)
        {
            Group group = await _groupRepository.GetById(groupId);
            group.ChangeStationName(stationId, name);
            await _groupRepository.Save(group);

            return group.Stations[stationId];
        }

        public async Task<ChargeStation> Create(Guid groupId, string name, decimal maxCurrent)
        {
            Group group = await _groupRepository.GetById(groupId);
            ChargeStation newStation = new ChargeStation(Guid.NewGuid(), name, maxCurrent);
            group.AddStation(newStation);
            await _groupRepository.Save(group);

            return newStation;
        }

        public async Task<ChargeStation> Get(Guid groupId, Guid stationId)
        {
            Group group = await _groupRepository.GetById(groupId);

            return group.Stations[stationId];
        }

        public async Task<ChargeStation> Remove(Guid groupId, Guid stationId)
        {
            Group group = await _groupRepository.GetById(groupId);
            ChargeStation station = group.RemoveStation(stationId);
            await _groupRepository.Save(group);

            return station;
        }
    }
}
