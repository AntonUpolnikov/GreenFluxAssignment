using GreenFluxAssignment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenFluxAssignment.Persistence.Configurations
{
    internal class ChargeStationConfiguration : IEntityTypeConfiguration<ChargeStation>
    {
        public void Configure(EntityTypeBuilder<ChargeStation> builder)
        {
            builder.ToTable("ChargeStations");

            builder.HasKey(cs => cs.Id)
                .HasName("PK_ChargeStations_Id");

            builder.Property(cs => cs.Id)
                .ValueGeneratedNever();

            builder.OwnsMany(cs => cs.Connectors, connectorBuilder =>
            {
                connectorBuilder
                    .WithOwner()
                    .HasForeignKey(c => c.ChargeStationId)
                    .HasConstraintName("FK_Connectors_ChargeStations_ChargeStationId");

                connectorBuilder.Property(c => c.Id)
                    .ValueGeneratedNever();

                connectorBuilder.HasKey(c => new { c.ChargeStationId, c.Id })
                    .HasName("PK_Connectors_ChargeStationId_Id");

                connectorBuilder.Property(c => c.MaxCurrent)
                    .HasColumnType("decimal(9,2)");
            });
        }
    }
}
