using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GreenFluxAssignment.Persistence.Repository
{
    public class DesignMigrationsContextFactory : IDesignTimeDbContextFactory<GroupContext>
    {
        public GroupContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GroupContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GreenFluxAssignment;Integrated Security=True;Pooling=True");

            return new GroupContext(optionsBuilder.Options);
        }
    }
}
