using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class Street
{
    public string? Id { get; set; }

    public string? ZipCode { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Name { get; set; }
    public string? DisplayName { get; set; }

    public uint LowestHouseNumber { get; set; } = 1;

    public uint HighestHouseNumber { get; set; } = uint.MaxValue;

    public string? DistributionTourId { get; set; }
    public DistributionTour? DistributionTour { get; private set; }
    public uint DistributionTourFormCount { get; set; }

    public string? CollectionTourId { get; set; }
    public CollectionTour? CollectionTour { get; private set; }
    public uint CollectionTourOrderNumber { get; set; }

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
            DisplayName = entity.DisplayName,
            LowestHouseNumber = entity.LowestHouseNumber,
            HighestHouseNumber = entity.HighestHouseNumber,
            DistributionTourId = entity.DistributionTour is null ? null : entity.DistributionTour.Id,
            DistributionTour = entity.DistributionTour is null ? null : DistributionTour.FromEntity(entity.DistributionTour),
            DistributionTourFormCount = entity.DistributionTourFormCount,
            CollectionTourId = entity.CollectionTour is null ? null : entity.CollectionTourId,
            CollectionTour = entity.CollectionTour is null ? null : CollectionTour.FromEntity(entity.CollectionTour),
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
            DistributionTourFormCount = DistributionTourFormCount,
            CollectionTourOrderNumber = CollectionTourOrderNumber
        };
    }
}