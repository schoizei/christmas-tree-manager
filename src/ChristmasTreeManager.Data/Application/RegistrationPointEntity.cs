using System.ComponentModel.DataAnnotations;

namespace ChristmasTreeManager.Data.Application;

public class RegistrationPointEntity : AuditableEntity
{
    [Key]
    public required string Id { get; set; }

    public required string Name { get; set; }

    public string Address { get; set; } = string.Empty;

    public IList<RegistrationEntity> Registrations { get; set; } = [];
}
