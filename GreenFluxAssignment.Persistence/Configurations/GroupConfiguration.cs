using GreenFluxAssignment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenFluxAssignment.Persistence.Configurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups");

            builder.HasKey(g => g.Id)
                .HasName("PK_Groups_Id");

            builder.Property(g => g.Id)
                .ValueGeneratedNever();

            builder.Property(g => g.Capacity)
                .HasColumnType("decimal(9,2)");

            builder.HasMany(g => g.ChargeStations)
                .WithOne()
                .HasForeignKey(cs => cs.GroupId)
                .HasConstraintName("FK_ChargeStations_Groups_GroupId");

            builder.Property(g => g.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}
