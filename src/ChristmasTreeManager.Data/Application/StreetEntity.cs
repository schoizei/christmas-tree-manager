using System.ComponentModel.DataAnnotations;

namespace ChristmasTreeManager.Data.Application;

public class StreetEntity : AuditableEntity
{
    [Key]
    public required string Id { get; set; }

    public required string ZipCode { get; set; }

    public required string City { get; set; }

    public required string District { get; set; }

    public required string Name { get; set; }

    public uint LowestHouseNumber { get; set; } = 1;

    public uint HighestHouseNumber { get; set; } = uint.MaxValue;

    public string? DistributionTourId { get; set; }
    public DistributionTourEntity? DistributionTour { get; set; }
    public uint DistributionTourOrderNumber { get; set; } = 0;

    public string? CollectionTourId { get; set; }
    public CollectionTourEntity? CollectionTour { get; set; }
    public uint CollectionTourOrderNumber { get; set; } = 0;

    public IList<RegistrationEntity> Registrations { get; set; } = [];
}
