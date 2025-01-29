using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektWebowy.Models;

namespace ProjektWebowy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Serial> Serials { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            builder.Entity<Comment>()
                .HasOne(c => c.Serial)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SerialId);

            builder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Rating>()
                .HasOne(r => r.Serial)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SerialId);
        }
    }
}
