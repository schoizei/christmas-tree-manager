using System.ComponentModel.DataAnnotations;

namespace ChristmasTreeManager.Data.Application;

public class DistributionTourEntity : AuditableEntity
{
    [Key]
    public required string Id { get; set; }

    public required string Name { get; set; }

    public string Staff { get; set; } = string.Empty;

    public IList<StreetEntity> Streets { get; set; } = [];
}
