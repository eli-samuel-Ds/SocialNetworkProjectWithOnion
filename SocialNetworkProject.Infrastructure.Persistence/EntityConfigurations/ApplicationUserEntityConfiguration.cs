using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("Users");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(u => u.Email)
                .IsUnique();
            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500);
        }
    }
}
