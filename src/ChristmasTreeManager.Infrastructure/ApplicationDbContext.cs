using ChristmasTreeManager.Data.Application;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTreeManager.Infrastructure;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

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