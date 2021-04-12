using System;
using System.Threading.Tasks;
using GreenFluxAssignment.Domain.Exceptions;
using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Persistence.Entities;
using GreenFluxAssignment.Persistence.Exceptions;
using GreenFluxAssignment.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GreenFluxAssignment.Persistence.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly GroupContext _groupContext;
        private readonly IEntityConverter _entityConverter;

        public GroupRepository(GroupContext groupContext, IEntityConverter entityConverter)
        {
            _groupContext = groupContext;
            _entityConverter = entityConverter;
        }

        public async Task<Domain.Entities.Group> GetById(Guid groupId)
        {
            Group group = await FindById(groupId);
            return _entityConverter.FromEntity(group);
        }

        public async Task<Domain.Entities.Group> RemoveById(Guid groupId)
        {
            try
            {
                Group group = await FindById(groupId);

                _groupContext.Groups.Remove(group);
                await _groupContext.SaveChangesAsync();

                return _entityConverter.FromEntity(group);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ConcurrencyConflictException(groupId.ToString(), exception);
            }
        }

        public async Task<Domain.Entities.Group> Save(Domain.Entities.Group group)
        {
            try
            {
                Group entity = await _groupContext.Groups.FindAsync(group.Id);
                Group updatedEntity = _entityConverter.ToEntity(group, entity);

                var entry = entity == null
                    ? _groupContext.Groups.Add(updatedEntity)
                    : _groupContext.Groups.Update(updatedEntity);

                await _groupContext.SaveChangesAsync();

                return group;
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ConcurrencyConflictException(group.Id.ToString(), exception);
            }
        }

        private async Task<Group> FindById(Guid groupId)
        {
            Group group = await _groupContext.Groups
                .Include(g => g.ChargeStations)
                .ThenInclude(s => s.Connectors)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group is null)
            {
                throw new EntityNotFoundException(nameof(Domain.Entities.Group), groupId.ToString());
            }

            return group;
        }
    }
}
