using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class Registration
{
    public string? Id { get; set; }

    public string? RegistrationPointId { get; set; }
    public string? RegistrationPointName { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public string Customer { get; set; } = string.Empty;

    public string? StreetId { get; set; }
    public string? Street { get; set; }

    public uint? Housenumber { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Mail { get; set; } = string.Empty;

    public uint TreeCount { get; set; } = 1;

    public double Donation { get; set; } = 0.0;

    public string Comment { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CreatedAt { get; set; }

    public static Registration FromEntity(RegistrationEntity entity)
    {
        return new Registration()
        {
            Id = entity.Id.ToString(),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt.ToString(),
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt.ToString(),
            RegistrationPointId = entity.RegistrationPointId,
            RegistrationPointName = entity.RegistrationPoint.Name,
            RegistrationDate = entity.RegistrationDate,
            Customer = entity.Customer,
            StreetId = entity.StreetId,
            Street = $"{entity.Street.City} - {entity.Street.Name}",
            Housenumber = entity.Housenumber,
            Phone = entity.Phone ?? string.Empty,
            Mail = entity.Mail ?? string.Empty,
            TreeCount = entity.TreeCount,
            Donation = entity.Donation,
            Comment = entity.Comment
        };
    }

    public RegistrationEntity ToEntity()
    {
        return new RegistrationEntity()
        {
            Id = Id is null ? Guid.NewGuid().ToString() : Id,
            RegistrationPointId = RegistrationPointId ?? throw new InvalidOperationException("Property [Registration.RegistrationPointId] is null!"),
            RegistrationDate = RegistrationDate,
            Customer = Customer ?? throw new InvalidOperationException("Property [Registration.Customer] is null!"),
            StreetId = StreetId ?? throw new InvalidOperationException("Property [Registration.StreetId] is null!"),
            Housenumber = Housenumber ?? throw new InvalidOperationException("Property [Registration.Housenumber] is null!"),
            Phone = Phone,
            Mail = Mail,
            TreeCount = TreeCount,
            Donation = Donation,
            Comment = Comment
        };
    }
}