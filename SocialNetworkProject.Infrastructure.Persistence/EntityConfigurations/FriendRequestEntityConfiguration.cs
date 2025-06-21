using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Infrastructure.Persistence.EntityConfigurations
{
    public class FriendRequestEntityConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(fr => fr.Id);
            builder.ToTable("FriendRequests");

            builder.Property(fr => fr.Status)
                .IsRequired();
            builder.Property(fr => fr.RequestedAt)
                .IsRequired();
            builder.Property(fr => fr.RespondedAt)
                .IsRequired(false);

            builder.HasOne(fr => fr.Requester)
                .WithMany(u => u.SentRequests)
                .HasForeignKey(fr => fr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(fr => new { fr.RequesterId, fr.ReceiverId })
                .IsUnique();
        }
    }
}
