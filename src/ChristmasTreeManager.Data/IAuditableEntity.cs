namespace ChristmasTreeManager.Data;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }

    string UpdatedBy { get; set; }

    DateTime CreatedAt { get; set; }

    DateTime UpdatedAt { get; set; }
}
