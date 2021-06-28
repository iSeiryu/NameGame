using Microsoft.EntityFrameworkCore;
using NameGame.Persistence.Models;

namespace NameGame.Persistence.DbContexts {
    public class NameGameContext : DbContext {
        public NameGameContext(DbContextOptions<NameGameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
    }
}
