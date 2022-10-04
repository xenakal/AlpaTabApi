using AlpaTabApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpaTabApi.Data
{
    public class AlpaTabContext : DbContext
    {
        public AlpaTabContext (DbContextOptions<AlpaTabContext> options)
            : base(options)
        {
        }

        public DbSet<AlpaTabUser> AlpaTabUsers { get; set; }
        public DbSet<AlpaTabTransaction> TransactionsList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<AlpaTabUser>().ToTable(nameof(AlpaTabUser));
            builder.Entity<AlpaTabTransaction>().ToTable(nameof(AlpaTabTransaction));
            builder.Entity<AlpaTabTransaction>(t => {t.HasKey(e => e.Id);});
        }
    }
}
