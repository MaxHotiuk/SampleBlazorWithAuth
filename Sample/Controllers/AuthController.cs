using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sample.Client.Models;
using Sample.Core.Entities;
using Sample.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

namespace Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserRepository userRepository, IConfiguration configuration) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userRepository.GetByUserNameAsync(model.Username);
        if (userExists != null)
            return BadRequest("User already exists!");

        var user = new User
        {
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userRepository.AddAsync(user, model.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User created successfully!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userRepository.GetByUserNameAsync(model.Username) 
            ?? await _userRepository.GetByEmailAsync(model.Username);
        if (user == null)
            return Unauthorized();

        var passwordValid = await _userRepository.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
            return Unauthorized();

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = GetToken(authClaims);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        Env.Load();
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET")!));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:ValidIssuer"],
            audience: _configuration["JwtSettings:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}