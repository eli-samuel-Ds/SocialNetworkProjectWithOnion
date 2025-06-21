using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class ShipEntityConfiguration : IEntityTypeConfiguration<Ship>
    {
        public void Configure(EntityTypeBuilder<Ship> builder)
        {
            builder.HasKey(s => s.Id);
            builder.ToTable("Ships");

            builder.Property(s => s.Type).IsRequired();
            builder.Property(s => s.Size).IsRequired();
            builder.Property(s => s.IsPositioned).IsRequired();

            builder.HasOne(s => s.Owner)
                .WithMany(u => u.Ships)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Positions)
                .WithOne(p => p.Ship)
                .HasForeignKey(p => p.ShipId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
