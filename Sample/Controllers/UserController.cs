using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Client.Models;
using Sample.Core.Entities;
using Sample.Core.Interfaces;
using System.Security.Claims;

namespace Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        return Ok(new
        {
            Username = user.UserName,
            Email = user.Email,
            HasProfilePicture = user.PfpContent != null && user.PfpContent.Length > 0
        });
    }
    
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        // Check if the new username is already taken by another user
        if (!string.Equals(user.UserName, model.Username, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await _userRepository.GetByUserNameAsync(model.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken");
            }
            
            user.UserName = model.Username;
            var result = await _userRepository.UpdateAsync(user);
            
            if (result != "Success")
            {
                return BadRequest(result);
            }
        }
        
        return Ok("Profile updated successfully");
    }
    
    [HttpPost("profilepicture")]
    public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded");
        }
        
        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest("Only JPG and PNG images are supported");
        }
        
        // Validate file size (max 2MB)
        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest("Image size must be less than 2MB");
        }
        
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            user.PfpContent = memoryStream.ToArray();
        }
        
        var result = await _userRepository.UpdateAsync(user);
        
        if (result != "Success")
        {
            return BadRequest(result);
        }
        
        return Ok("Profile picture uploaded successfully");
    }
    
    [HttpDelete("profilepicture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user == null)
        {
            return NotFound("User not found");
        }
        
        user.PfpContent = null;
        var result = await _userRepository.UpdateAsync(user);
        
        if (result != "Success")
        {
            return BadRequest(result);
        }
        
        return Ok("Profile picture removed successfully");
    }

    [HttpGet("profilepicture")]
    public async Task<IActionResult> GetProfilePicture()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user == null || user.PfpContent == null || user.PfpContent.Length == 0)
        {
            return NotFound();
        }
        
        string contentType = DetermineImageContentType(user.PfpContent);
        
        return File(user.PfpContent, contentType);
    }

    private string DetermineImageContentType(byte[] imageData)
    {
        // Check for PNG signature
        if (imageData.Length > 8 && 
            imageData[0] == 0x89 && 
            imageData[1] == 0x50 && 
            imageData[2] == 0x4E && 
            imageData[3] == 0x47)
        {
            return "image/png";
        }
        
        // Check for JPEG signature
        if (imageData.Length > 3 &&
            imageData[0] == 0xFF &&
            imageData[1] == 0xD8 &&
            imageData[2] == 0xFF)
        {
            return "image/jpeg";
        }
        
        // Default to JPEG
        return "image/jpeg";
    }
}