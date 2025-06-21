using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class BattleEntityConfiguration : IEntityTypeConfiguration<Battle>
    {
        public void Configure(EntityTypeBuilder<Battle> builder)
        {
            builder.HasKey(b => b.Id);
            builder.ToTable("Battles");

            builder.Property(b => b.Status)
                .IsRequired();
            builder.Property(b => b.StartedAt)
                .IsRequired();
            builder.Property(b => b.EndedAt)
                .IsRequired(false);

            builder.HasOne(b => b.Player1)
                .WithMany(u => u.BattlesAsPlayer1)
                .HasForeignKey(b => b.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Player2)
                .WithMany(u => u.BattlesAsPlayer2)
                .HasForeignKey(b => b.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Ships)
                .WithOne(s => s.Battle)
                .HasForeignKey(s => s.BattleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Attacks)
                .WithOne(a => a.Battle)
                .HasForeignKey(a => a.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
