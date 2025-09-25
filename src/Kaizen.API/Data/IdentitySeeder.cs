using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Kaizen.API.Data;

public static class IdentitySeeder
{
    /// <summary>
    /// Seeds the admin user in the database from the given email and password in configuration.
    /// </summary>
    /// <returns><c>true</c> if new admin user was created, otherwise <c>false</c> (admin user already exists).</returns>
    public static Task<bool> SeedAdminUser(IServiceProvider services, IConfiguration config)
    {
        var email = config["AdminUser:Email"];
        var password = config["AdminUser:Password"];

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new InvalidOperationException("AdminUser:Email and AdminUser:Password must both be configured.");

        return SeedAdminUser(services, email, password);
    }

    /// <summary>
    /// Seeds the admin user in the database with the given email and password.
    /// </summary>
    /// <returns><c>true</c> if new admin user was created, otherwise <c>false</c> (admin user already exists).</returns>
    public static Task<bool> SeedAdminUser(IServiceProvider services, string email, string password)
    {
        return SeedUser(services, email, password, AuthConstants.Roles.Admin);
    }

    /// <summary>
    /// Seeds the user in the database with the given email, password and role.
    /// </summary>
    /// <returns><c>true</c> if the new user was created, otherwise <c>false</c> (user with email exists).</returns>
    public static async Task<bool> SeedUser(
        IServiceProvider services,
        string email,
        string password,
        string role = AuthConstants.Roles.User)
    {
        var userManager = services.GetRequiredService<UserManager<KaizenUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var roleExists = await roleManager.RoleExistsAsync(role);

        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return false;
        }

        var user = new KaizenUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(
                $"Failed to seed {role} user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        await userManager.AddToRoleAsync(user, role);

        return true;
    }
}