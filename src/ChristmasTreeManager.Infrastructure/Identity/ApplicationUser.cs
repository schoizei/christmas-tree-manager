using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChristmasTreeManager.Infrastructure.Identity;

public partial class ApplicationUser : IdentityUser
{
    [JsonIgnore, IgnoreDataMember]
    public override string? PasswordHash { get; set; }

    [NotMapped]
    public string? Password { get; set; }

    [NotMapped]
    public string? ConfirmPassword { get; set; }

    public ICollection<ApplicationRole> Roles { get; set; } = [];
}