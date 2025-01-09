namespace ChristmasTreeManager.Models;

public class DistributionTourExport
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string Staff { get; set; } = string.Empty;

    public List<Street> Streets { get; set; } = [];

    public static DistributionTourExport Create(DistributionTour tour, List<Street> streets)
    {
        return new DistributionTourExport()
        {
            Id = tour.Id,
            Name = tour.Name,
            Staff = tour.Staff,
            Streets = streets
        };
    }
}