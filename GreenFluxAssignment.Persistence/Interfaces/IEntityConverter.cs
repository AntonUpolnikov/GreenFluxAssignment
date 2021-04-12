
using GreenFluxAssignment.Persistence.Entities;

namespace GreenFluxAssignment.Persistence.Interfaces
{
    public interface IEntityConverter
    {
        Domain.Entities.Group FromEntity(Group entity);

        Group ToEntity(Domain.Entities.Group group, Group entity = null);
    }
}
