namespace ChristmasTreeManager.Configuration;

public record DatabaseProvider(string Name, string Assembly)
{
    public static readonly DatabaseProvider Sqlite = new(nameof(Sqlite), typeof(Infrastructure.Sqlite.ProviderMarker).Assembly.GetName().Name!);
    public static readonly DatabaseProvider Postgres = new(nameof(Postgres), typeof(Infrastructure.Postgres.ProviderMarker).Assembly.GetName().Name!);
    public static readonly DatabaseProvider MySql = new(nameof(MySql), typeof(Infrastructure.MySql.ProviderMarker).Assembly.GetName().Name!);
}
