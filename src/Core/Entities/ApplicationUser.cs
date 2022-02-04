using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public bool IsActive { get; set; } = true;
}
