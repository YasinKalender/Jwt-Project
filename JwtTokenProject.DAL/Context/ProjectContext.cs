using JwtTokenProject.DAL.Configuration;
using JwtTokenProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JwtTokenProject.DAL.Context
{
    public class ProjectContext : IdentityDbContext<UserApp, IdentityRole, string>
    {
        public ProjectContext(DbContextOptions<ProjectContext> dbContext) : base(dbContext)
        {

        }
        public DbSet<UserApp> UserApp { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            //builder.Entity<UserApp>().OwnsOne(i => i.Id);
            builder.Entity<UserApp>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<IdentityUserLogin<string>>().HasNoKey();
            builder.Entity<IdentityUserRole<string>>().HasNoKey();
            builder.Entity<IdentityUserToken<string>>().HasNoKey();

            //builder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
