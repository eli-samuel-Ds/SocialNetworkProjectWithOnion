using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class ShipPositionEntityConfiguration : IEntityTypeConfiguration<ShipPosition>
    {
        public void Configure(EntityTypeBuilder<ShipPosition> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("ShipPositions");

            builder.Property(p => p.X).IsRequired();
            builder.Property(p => p.Y).IsRequired();

            builder.HasOne(p => p.Ship)
                .WithMany(s => s.Positions)
                .HasForeignKey(p => p.ShipId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
