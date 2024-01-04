using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class DistributionTour
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string Staff { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CreatedAt { get; set; }


    public static DistributionTour FromEntity(DistributionTourEntity entity)
    {
        return new DistributionTour()
        {
            Id = entity.Id.ToString(),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt.ToString(),
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt.ToString(),
            Name = entity.Name,
            Staff = entity.Staff
        };
    }

    public DistributionTourEntity ToEntity()
    {
        return new DistributionTourEntity()
        {
            Id = Id is null ? Guid.NewGuid().ToString() : Id,
            Name = Name ?? throw new InvalidOperationException("Property [RegistrationPoint.Name] is null!"),
            Staff = Staff
        };
    }
}