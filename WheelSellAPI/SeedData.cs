using Microsoft.AspNetCore.Identity;
using WheelSell.DAL.Entities;

namespace WheelSell.API
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }
            if (!await roleManager.RoleExistsAsync("Saler"))
            {
                await roleManager.CreateAsync(new Role { Name = "Saler" });
            }

            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}