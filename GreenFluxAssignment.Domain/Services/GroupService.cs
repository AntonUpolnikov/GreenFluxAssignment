using System;
using System.Threading.Tasks;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Domain.Interfaces.Services;

namespace GreenFluxAssignment.Domain.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Group> Create(string name, decimal capacity)
        {
            Group group = new Group(Guid.NewGuid(), name, capacity);
            return await _groupRepository.Save(group);
        }

        public async Task<Group> Get(Guid groupId)
        {
            return await _groupRepository.GetById(groupId);
        }

        public async Task<Group> Remove(Guid groupId)
        {
            return await _groupRepository.RemoveById(groupId);
        }

        public async Task<Group> Update(Guid groupId, string name = null, decimal? capacity = null)
        {
            Group group = await _groupRepository.GetById(groupId);

            if (!string.IsNullOrWhiteSpace(name))
            {
                group.ChangeName(name);
            }

            if (capacity != null)
            {
                group.ChangeCapacity(capacity.Value);
            }

            return await _groupRepository.Save(group);
        }
    }
}
