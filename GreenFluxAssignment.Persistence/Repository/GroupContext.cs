using GreenFluxAssignment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFluxAssignment.Persistence.Repository
{
    public class GroupContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }

        public GroupContext(DbContextOptions<GroupContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GroupContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
