using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class RegistrationPoint
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CreatedAt { get; set; }


    public static RegistrationPoint FromEntity(RegistrationPointEntity entity)
    {
        return new RegistrationPoint()
        {
            Id = entity.Id.ToString(),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt.ToString(),
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt.ToString(),
            Name = entity.Name,
            Address = entity.Address
        };
    }

    public RegistrationPointEntity ToEntity()
    {
        return new RegistrationPointEntity()
        {
            Id = Id is null ? Guid.NewGuid().ToString() : Id,
            Name = Name ?? throw new InvalidOperationException("Property [RegistrationPoint.Name] is null!"),
            Address = Address ?? throw new InvalidOperationException("Property [RegistrationPoint.Address] is null!"),
        };
    }
}