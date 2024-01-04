using System.ComponentModel.DataAnnotations;

namespace ChristmasTreeManager.Data.Application;

public class RegistrationEntity : AuditableEntity
{
    [Key]
    public required string Id { get; set; }

    public required string RegistrationPointId { get; set; }
    public RegistrationPointEntity RegistrationPoint { get; set; } = null!;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public required string Customer { get; set; }

    public required string StreetId { get; set; }
    public StreetEntity Street { get; set; } = null!;

    public required uint Housenumber { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Mail { get; set; } = string.Empty;

    public uint TreeCount { get; set; } = 1;

    public double Donation { get; set; } = 0.0;

    public string Comment { get; set; } = string.Empty;

    public string DisplayHousenumber
    {
        get
        {
            var result = Housenumber.ToString() ?? string.Empty;

            return result;
        }
    }
}
