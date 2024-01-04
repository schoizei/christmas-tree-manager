using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ChristmasTreeManager.Infrastructure.Identity;

public partial class ApplicationRole : IdentityRole
{
    [JsonIgnore]
    public ICollection<ApplicationUser> Users { get; set; } = [];
}