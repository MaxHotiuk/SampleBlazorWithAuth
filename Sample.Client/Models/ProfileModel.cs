using System.ComponentModel.DataAnnotations;

namespace Sample.Client.Models;

public class ProfileModel
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public bool HasProfilePicture { get; set; }
}

public class UpdateProfileModel
{
    public string Username { get; set; } = string.Empty;
}