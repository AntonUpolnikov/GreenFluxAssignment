using System;
using System.Threading.Tasks;
using GreenFluxAssignment.Domain.Entities;

namespace GreenFluxAssignment.Domain.Interfaces.Repository
{
    public interface IGroupRepository
    {
        public Task<Group> GetById(Guid groupId);
        public Task<Group> Save(Group group);
        public Task<Group> RemoveById(Guid groupId);
    }
}
