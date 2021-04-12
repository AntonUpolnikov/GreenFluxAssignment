using GreenFluxAssignment.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Domain.Interfaces.Services
{
    public interface IGroupService
    {
        Task<Group> Create(string name, decimal capacity);
        Task<Group> Remove(Guid groupId);
        Task<Group> Update(Guid groupId, string name = null, decimal? capacity = null);
        Task<Group> Get(Guid groupId);
    }
}
