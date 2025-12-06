using ChristmasTreeManager.Components;
using ChristmasTreeManager.Configuration;
using ChristmasTreeManager.Infrastructure;
using ChristmasTreeManager.Infrastructure.Identity;
using ChristmasTreeManager.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
});

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHotKeys2();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "ChristmasTreeManagerTheme";
    options.Duration = TimeSpan.FromDays(365);
});

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
     {
         var connectionString = builder.Configuration.GetConnectionString("Application");
         var databaseProvider = builder.Configuration.GetValue(nameof(DatabaseProvider), DatabaseProvider.Sqlite.Name);
         if (databaseProvider == DatabaseProvider.Sqlite.Name)
         {
             Console.WriteLine($"[ApplicationDbContext] Use DatabaseProvider.Sqlite with ConnectionString [{connectionString}]");
             options.UseSqlite(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Sqlite.Assembly));
         }
         else if (databaseProvider == DatabaseProvider.Postgres.Name)
         {
             Console.WriteLine($"[ApplicationDbContext] Use DatabaseProvider.Postgres with ConnectionString [{connectionString}]");
             options.UseNpgsql(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Postgres.Assembly));
         }
         else
         {
             throw new InvalidOperationException("No valid database provider found!");
         }
     })
    .AddScoped<ApplicationDbService>()
    .AddScoped<ExportService>();

builder.Services
    .AddDbContext<IdentityDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Application");
        var databaseProvider = builder.Configuration.GetValue(nameof(DatabaseProvider), DatabaseProvider.Sqlite.Name);
        if (databaseProvider == DatabaseProvider.Sqlite.Name)
        {
            Console.WriteLine($"[IdentityDbContext] Use DatabaseProvider.Sqlite with ConnectionString [{connectionString}]");
            options.UseSqlite(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Sqlite.Assembly));
        }
        else if (databaseProvider == DatabaseProvider.Postgres.Name)
        {
            Console.WriteLine($"[IdentityDbContext] Use DatabaseProvider.Postgres with ConnectionString [{connectionString}]");
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly(DatabaseProvider.Postgres.Assembly));
        }
        else
        {
            throw new InvalidOperationException("No valid database provider found!");
        }
    })
    .AddScoped<SecurityService>()
    .AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddHttpClient("ChristmasTreeManager", (sp, client) =>
    {
        var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is not null)
        {
            // Prefer X-Forwarded-Proto header (set by reverse proxy), then check IsHttps, default to https
            var forwardedProto = request.Headers["X-Forwarded-Proto"].FirstOrDefault();
            var scheme = !string.IsNullOrEmpty(forwardedProto) 
                ? forwardedProto 
                : (request.IsHttps ? "https" : "https"); // Default to https for security
            var uri = new Uri($"{scheme}://{request.Host}");
            client.BaseAddress = uri;
        }
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false })
    .AddHeaderPropagation(o => o.Headers.Add("Cookie"));

builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services
    .AddControllers()
    .AddOData(o =>
    {
        var oDataBuilder = new ODataConventionModelBuilder();

        oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
        var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
        usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
        usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));

        oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");

        o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
    });

var app = builder.Build();
app.UseForwardedHeaders();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapControllers();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

try
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();
    scope.ServiceProvider.GetRequiredService<IdentityDbContext>().SeedSupperUser();
}
catch
{
    //NOP
}

await app.RunAsync();
