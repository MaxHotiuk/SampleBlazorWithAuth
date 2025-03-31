using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Core.Entities;
using Sample.Infrastructure.Data;

namespace Sample.Infrastructure.Seed;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.Migrate();

        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }
    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    private static async Task SeedUsers(UserManager<User> userManager)
    {
        if (await userManager.FindByEmailAsync("admin@example.com") == null)
        {
            var admin = new User
            {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin1$");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (await userManager.FindByEmailAsync("user1@example.com") == null)
        {
            var user1 = new User
            {
            UserName = "user1",
            Email = "user1@example.com",
            EmailConfirmed = true
            };
            await userManager.CreateAsync(user1, "User1$");
            await userManager.AddToRoleAsync(user1, "User");
        }

        if (await userManager.FindByEmailAsync("user2@example.com") == null)
        {
            var user2 = new User
            {
            UserName = "user2",
            Email = "user2@example.com",
            EmailConfirmed = true
            };
            await userManager.CreateAsync(user2, "User2$");
            await userManager.AddToRoleAsync(user2, "User");
        }
    }
}

        