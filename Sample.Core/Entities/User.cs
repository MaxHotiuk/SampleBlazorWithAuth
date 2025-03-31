using Microsoft.AspNetCore.Identity;

namespace Sample.Core.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? BannedTo { get; set; }

    public string ProfilePicture { get; set; } = "pfp_1.png";
}
