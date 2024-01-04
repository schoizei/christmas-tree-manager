using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class Street
{
    public string? Id { get; set; }

    public string? ZipCode { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Name { get; set; }

    public uint LowestHouseNumber { get; set; } = 1;

    public uint HighestHouseNumber { get; set; } = uint.MaxValue;

    public string? DistributionTourId { get; set; }
    public string? DistributionTourName { get; set; }
    public uint DistributionTourOrderNumber { get; set; } = 0;

    public string? CollectionTourId { get; set; }
    public string? CollectionTourName { get; set; }
    public uint CollectionTourOrderNumber { get; set; } = 0;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CreatedAt { get; set; }

    public static Street FromEntity(StreetEntity entity)
    {
        return new Street()
        {
            Id = entity.Id.ToString(),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt.ToString(),
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt.ToString(),
            ZipCode = entity.ZipCode,
            City = entity.City,
            District = entity.District,
            Name = entity.Name,
            LowestHouseNumber = entity.LowestHouseNumber,
            HighestHouseNumber = entity.HighestHouseNumber,
            DistributionTourId = entity.DistributionTour?.Id.ToString(),
            DistributionTourName = entity.DistributionTour?.Name,
            DistributionTourOrderNumber = entity.DistributionTourOrderNumber,
            CollectionTourId = entity.CollectionTour?.Id.ToString(),
            CollectionTourName = entity.CollectionTour?.Name,
            CollectionTourOrderNumber = entity.CollectionTourOrderNumber
        };
    }

    public StreetEntity ToEntity()
    {
        return new StreetEntity()
        {
            Id = Id is null ? Guid.NewGuid().ToString() : Id,
            Name = Name ?? throw new InvalidOperationException("Property [RegistrationPoint.Name] is null!"),
            ZipCode = ZipCode ?? throw new InvalidOperationException("Property [RegistrationPoint.ZipCode] is null!"),
            City = City ?? throw new InvalidOperationException("Property [RegistrationPoint.City] is null!"),
            District = District ?? throw new InvalidOperationException("Property [RegistrationPoint.District] is null!"),
            LowestHouseNumber = LowestHouseNumber,
            HighestHouseNumber = HighestHouseNumber,
            DistributionTourId = DistributionTourId,
            DistributionTourOrderNumber = DistributionTourOrderNumber,
            CollectionTourId = CollectionTourId,
            CollectionTourOrderNumber = CollectionTourOrderNumber
        };
    }
}