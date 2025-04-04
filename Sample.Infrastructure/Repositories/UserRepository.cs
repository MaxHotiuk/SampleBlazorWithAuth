using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sample.Core.Entities;
using Sample.Core.Interfaces;
using Sample.Infrastructure.Data;

namespace Sample.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : RepositoryBase<User>(context), IUserRepository
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<IdentityResult> AddAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (_roleManager.Roles.All(r => r.Name != "User"))
        {
            var role = new IdentityRole("User");
            await _roleManager.CreateAsync(role);
        }
        await _userManager.AddToRoleAsync(user, "User");
        return result;
    }

    public async override Task<string> AddAsync(User user)
    {
        var result = await _userManager.CreateAsync(user);
        return result.Succeeded ? "Success" : string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async override Task<string> UpdateAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? "Success" : string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async Task<IdentityResult> UpdateAsync(User user, string password)
    {
        await _userManager.UpdateAsync(user);
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, password);
        return IdentityResult.Success;
    }

    public async Task<string> DeleteAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded ? "Success" : string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        return usersInRole.AsEnumerable();
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
