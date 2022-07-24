using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager
        )
        {
            if (await userManager.Users.AnyAsync()) return;

            const string userDataPath = "Data/SeedData/UserSeedData.json";
            var userData = await System.IO.File.ReadAllTextAsync(userDataPath);
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            Debug.Assert(users != null, nameof(users) + " != null");

            var roles = new List<AppRole>
            {
                new() { Name = "Admin" },
                new() { Name = "Moderator" },
                new() { Name = "Member" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "password");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "Admin"
            };

            await userManager.CreateAsync(admin, "password");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
    }
}

/*=================================== old way
 
foreach (var user in users)
{
    // using var hmac = new HMACSHA512();

    user.UserName = user.UserName.ToLower();

    // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
    // user.PasswordSalt = hmac.Key;
    // context.Users.Add(user);

    await userManager.CreateAsync(user, "password");
}

// await context.SaveChangesAsync();
================================================*/