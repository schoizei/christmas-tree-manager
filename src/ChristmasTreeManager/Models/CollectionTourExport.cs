namespace ChristmasTreeManager.Models;

public class CollectionTourExport
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Vehicle { get; set; } = string.Empty;

    public string Driver { get; set; } = string.Empty;

    public string TeamLeader { get; set; } = string.Empty;

    public string Staff { get; set; } = string.Empty;

    public List<RegistrationExport> Registrations { get; set; } = [];

    public static CollectionTourExport Create(CollectionTour tour, List<RegistrationExport> registrations)
    {
        return new CollectionTourExport()
        {
            Id = tour.Id?.ToString() ?? string.Empty,
            Name = tour.Name ?? string.Empty,
            Vehicle = tour.Vehicle,
            Driver = tour.Driver,
            TeamLeader = tour.TeamLeader,
            Staff = tour.Staff,
            Registrations = registrations
        };
    }
}