using System.ComponentModel.DataAnnotations;

namespace ChristmasTreeManager.Data.Application;

public class CollectionTourEntity : AuditableEntity
{
    [Key]
    public required string Id { get; set; }

    public required string Name { get; set; }

    public string Vehicle { get; set; } = string.Empty;

    public string Driver { get; set; } = string.Empty;

    public string Staff { get; set; } = string.Empty;

    public IList<StreetEntity> Streets { get; set; } = [];
}
