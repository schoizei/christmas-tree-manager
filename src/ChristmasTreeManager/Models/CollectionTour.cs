using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class CollectionTour
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string Vehicle { get; set; } = string.Empty;

    public string Driver { get; set; } = string.Empty;

    public string Staff { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CreatedAt { get; set; }


    public static CollectionTour FromEntity(CollectionTourEntity entity)
    {
        return new CollectionTour()
        {
            Id = entity.Id.ToString(),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt.ToString(),
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt.ToString(),
            Name = entity.Name,
            Vehicle = entity.Vehicle,
            Driver = entity.Driver,
            Staff = entity.Staff
        };
    }

    public CollectionTourEntity ToEntity()
    {
        return new CollectionTourEntity()
        {
            Id = Id is null ? Guid.NewGuid().ToString() : Id,
            Name = Name ?? throw new InvalidOperationException("Property [RegistrationPoint.Name] is null!"),
            Vehicle = Vehicle,
            Driver = Driver,
            Staff = Staff
        };
    }
}