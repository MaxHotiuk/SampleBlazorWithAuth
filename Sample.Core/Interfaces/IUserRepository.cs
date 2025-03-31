using Sample.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Sample.Core.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string userName);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    Task<IdentityResult> AddAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user, string password);
    Task<string> DeleteAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
}
