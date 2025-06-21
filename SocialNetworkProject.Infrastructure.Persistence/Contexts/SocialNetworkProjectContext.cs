using Microsoft.EntityFrameworkCore;
using SocialNetworkProject.Core.Domain.Entities;
using System.Reflection;

namespace SocialNetworkProject.Infrastructure.Persistence.Contexts
{
    public class SocialNetworkProjectContext : DbContext
    {
        public SocialNetworkProjectContext(DbContextOptions<SocialNetworkProjectContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Attack> Attack { get; set; }
        public DbSet<Battle> Battle { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<FriendRequest> FriendRequest { get; set; }
        public DbSet<Friendship> Friendship { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostReaction> PostReaction { get; set; }
        public DbSet<Ship> Ship { get; set; }
        public DbSet<ShipPosition> ShipPosition { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}