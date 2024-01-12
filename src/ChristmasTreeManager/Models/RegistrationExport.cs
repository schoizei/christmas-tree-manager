using ChristmasTreeManager.Data.Application;

namespace ChristmasTreeManager.Models;

public class RegistrationExport
{
    public string Id { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Customer { get; set; } = string.Empty;

    public uint TreeCount { get; set; } = 1;

    public string Comment { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Mail { get; set; } = string.Empty;

    public static RegistrationExport FromEntity(RegistrationEntity entity)
    {
        return new RegistrationExport()
        {
            Id = entity.Id,
            Address = $"{entity.Street.City} | {entity.Street.Name} {entity.Housenumber}{entity.HousenumberPostfix}",
            Customer = entity.Customer,
            TreeCount = entity.TreeCount,
            Comment = entity.Comment,
            Phone = entity.Phone,
            Mail = entity.Mail
        };
    }
}