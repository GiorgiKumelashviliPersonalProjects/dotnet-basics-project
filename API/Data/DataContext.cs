using System.Diagnostics.CodeAnalysis;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserLikes> Likes { get; set; }

        [SuppressMessage("ReSharper", "RedundantLambdaParameterType")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany((AppUser user) => user.UserRoles)
                .WithOne((AppUserRole user) => user.User)
                .HasForeignKey((AppUserRole user) => user.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany((AppRole user) => user.UserRoles)
                .WithOne((AppUserRole user) => user.Role)
                .HasForeignKey((AppUserRole user) => user.RoleId)
                .IsRequired();


            modelBuilder.Entity<UserLikes>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            modelBuilder.Entity<UserLikes>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLikes>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}