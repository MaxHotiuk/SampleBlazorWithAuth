using Microsoft.AspNetCore.Identity;

namespace Sample.Core.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? BannedTo { get; set; }

    public byte[]? PfpContent { get; set; }
}
