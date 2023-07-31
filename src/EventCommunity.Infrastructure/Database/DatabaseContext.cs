using EventCommunity.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventCommunity.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostFile> PostFiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RegisterRequest> RegisterRequests { get; set; }
        public DbSet<CommunityEvent> CommunityEvents { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
