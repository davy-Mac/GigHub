using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) // implements fluent API
        {
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.Gig)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)  // An application user has many Followers
                .WithRequired(f => f.Followee) // has a required Followee
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followees) // An application user has many Followees
                .WithRequired(f => f.Follower)// has a required Follower 
                .WillCascadeOnDelete(false);


            base.OnModelCreating(modelBuilder);
        }
    }
}
