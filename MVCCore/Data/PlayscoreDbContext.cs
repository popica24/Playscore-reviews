using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCCore.Models;

namespace MVCCore.Data
{
    public class PlayscoreDbContext : IdentityDbContext<UserModel>
    {
        public PlayscoreDbContext(DbContextOptions<PlayscoreDbContext> options) : base(options)
        {
           
        }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReviewModel>()
             .Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<ReviewModel>()
                .Property(x => x.DateAdded)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<ReviewModel>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId); //One Game to many Reviews

            modelBuilder.Entity<ReviewModel>()
                .HasOne(u => u.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(u => u.UserId);//Many reviews to one user

            modelBuilder.Entity<ReviewModel>()
           .HasIndex(r => new {r.UserId,r.GameId})
           .IsUnique(); // Composite unique index

            modelBuilder.Entity<GameModel>()
                 .Property(x => x.Id)
                 .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<GameModel>()
               .Property(u => u.AgeRating)
               .HasConversion<string>();

            modelBuilder.Entity<GameModel>()
               .Property(u => u.Category)
               .HasConversion<string>();
        }
    }
}
