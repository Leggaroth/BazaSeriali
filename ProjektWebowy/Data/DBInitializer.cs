using Microsoft.AspNetCore.Identity;
using ProjektWebowy.Models;

namespace ProjektWebowy.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed Roles
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminUser = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
            };

            string adminPassword = "Admin123!";
            var user = await userManager.FindByEmailAsync("admin@admin.com");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Anime" },
                    new Category { Name = "Fantasy" },
                    new Category { Name = "History" },
                    new Category { Name = "Action" },
                    new Category { Name = "Mystery" }
                );
                await context.SaveChangesAsync();
            }
            if (!context.Serials.Any())
            {
                context.Serials.AddRange(
                    new Serial { Title = "Fullmetal Alchemist", Description = "Bardzo dobre anime polecam", CategoryId = 1, Year = 2009 },
                    new Serial { Title = "Gintama", Description = "Bardzo dobra komedia", CategoryId = 1, Year = 2001 },
                    new Serial { Title = "Bleach", Description = "Długi tasiemiec", CategoryId = 1, Year = 2003 },
                    new Serial { Title = "Naruto", Description = "Podobno dobry tasiemiec", CategoryId = 1, Year = 2001 },
                    new Serial { Title = "Naruto Shippuden", Description = "Dobra kontynuacja tasiemca", CategoryId = 1, Year = 2009 },
                    new Serial { Title = "One Piece", Description = "Za długi tasiemiec", CategoryId = 1, Year = 1995 },
                    new Serial { Title = "Test", Description = "Nie ogladam seriali :(", CategoryId = 2, Year = 2009 },
                    new Serial { Title = "Test 2", Description = "Nie ogladam seriali :(", CategoryId = 3, Year = 2009 },
                    new Serial { Title = "Test 3", Description = "Nie ogladam seriali :(", CategoryId = 4, Year = 2009 },
                    new Serial { Title = "Test 4", Description = "Nie ogladam seriali :(", CategoryId = 5, Year = 2009 }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
