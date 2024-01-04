using ChristmasTreeManager.Components;
using ChristmasTreeManager.Configuration;
using ChristmasTreeManager.Infrastructure;
using ChristmasTreeManager.Infrastructure.Identity;
using ChristmasTreeManager.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Radzen;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddRadzenComponents()
    .AddHttpClient();

builder.Services
    .AddHttpClient("ChristmasTreeManager")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie")); ;

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Application");
        var databaseProvider = builder.Configuration.GetValue(nameof(DatabaseProvider), DatabaseProvider.Sqlite.Name);
        if (databaseProvider == DatabaseProvider.Sqlite.Name)
        {
            options.UseSqlite(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Sqlite.Assembly));
        }
        else if (databaseProvider == DatabaseProvider.Postgres.Name)
        {
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Postgres.Assembly));
        }
        else
        {
            throw new Exception("No valid database provider found!");
        }
    })
    .AddScoped<ApplicationDbService>();


builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services
    .AddDbContext<IdentityDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Application");
        var databaseProvider = builder.Configuration.GetValue(nameof(DatabaseProvider), DatabaseProvider.Sqlite.Name);
        if (databaseProvider == DatabaseProvider.Sqlite.Name)
        {
            options.UseSqlite(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Sqlite.Assembly));
        }
        else if (databaseProvider == DatabaseProvider.Postgres.Name)
        {
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Postgres.Assembly));
        }
        else
        {
            throw new Exception("No valid database provider found!");
        }
    })
    .AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>()
    .AddScoped<SecurityService>()
    .AddScoped<ExportService>();

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddControllers()
    .AddOData(options =>
    {
        var modelBuilder = new ODataConventionModelBuilder();

        modelBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
        var usersType = modelBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
        usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
        usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));

        modelBuilder.EntitySet<ApplicationRole>("ApplicationRoles");

        options.AddRouteComponents("odata/Identity", modelBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
scope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.Migrate();

app.Run();