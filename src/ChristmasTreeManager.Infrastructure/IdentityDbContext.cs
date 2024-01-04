using ChristmasTreeManager.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTreeManager.Infrastructure;

public partial class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public IdentityDbContext()
    {
    }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
               .HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<IdentityUserRole<string>>();

        this.OnModelBuilding(builder);
    }


    public void SeedSupperUser()
    {
        var superUser = new ApplicationUser()
        {
            UserName = "SuperUser",
            NormalizedUserName = "SUPERUSER",
            Email = "SuperUser",
            NormalizedEmail = "SUPERUSER",
            EmailConfirmed = true,
            LockoutEnabled = false
        };
        superUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(superUser, "15w@MvYdFY@n");

        if (!Users.Any(x => x.UserName == superUser.UserName))
        {
            Users.Add(superUser);
            SaveChanges();
        }
    }
}
