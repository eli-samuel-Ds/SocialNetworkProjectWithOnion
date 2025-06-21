using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class AttackEntityConfiguration : IEntityTypeConfiguration<Attack>
    {
        public void Configure(EntityTypeBuilder<Attack> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("Attacks");

            builder.Property(a => a.X).IsRequired();
            builder.Property(a => a.Y).IsRequired();
            builder.Property(a => a.IsHit).IsRequired();
            builder.Property(a => a.AttackedAt).IsRequired();

            builder.HasOne(a => a.Attacker)
                .WithMany(u => u.Attacks)
                .HasForeignKey(a => a.AttackerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
