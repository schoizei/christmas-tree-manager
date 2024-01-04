using ChristmasTreeManager.Data.Application;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTreeManager.Infrastructure;

//dotnet ef migrations add Initial --context ApplicationDbContext --project../ChristmasTreeManager.Infrastructure --output-dir Migrations\Application
//dotnet ef migrations add Initial --context IdentityDbContext --project../ChristmasTreeManager.Infrastructure --output-dir Migrations\Identity

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //partial void OnModelBuilding(ModelBuilder builder);

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<Street>()
    //      .HasOne(i => i.CollectionTour)
    //      .WithMany(i => i.Streets)
    //      .HasForeignKey(i => i.CollectionTourId)
    //      .HasPrincipalKey(i => i.Id);

    //    builder.Entity<Street>()
    //      .HasOne(i => i.DistributionTour)
    //      .WithMany(i => i.Streets)
    //      .HasForeignKey(i => i.DistributionTourId)
    //      .HasPrincipalKey(i => i.Id);
    //    OnModelBuilding(builder);
    //}

    public DbSet<StreetEntity> Streets { get; set; }

    public DbSet<DistributionTourEntity> DistributionTours { get; set; }

    public DbSet<CollectionTourEntity> CollectionTours { get; set; }

    public DbSet<RegistrationPointEntity> RegistrationPoints { get; set; }

    public DbSet<RegistrationEntity> Registrations { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
}