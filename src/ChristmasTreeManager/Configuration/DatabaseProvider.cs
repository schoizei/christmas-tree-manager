namespace ChristmasTreeManager.Configuration;

public record DatabaseProvider(string Name, string Assembly)
{
    public static readonly DatabaseProvider Sqlite = new(nameof(Sqlite), typeof(Infrastructure.Sqlite.ProviderMarker).Assembly.GetName().Name!);
    public static readonly DatabaseProvider Postgres = new(nameof(Postgres), typeof(Infrastructure.Postgres.ProviderMarker).Assembly.GetName().Name!);
    public static readonly DatabaseProvider MsSql = new(nameof(MsSql), typeof(Infrastructure.MsSql.ProviderMarker).Assembly.GetName().Name!);
}
