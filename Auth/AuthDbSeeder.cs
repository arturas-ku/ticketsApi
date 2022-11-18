using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportAPI.Auth.Model;
using SupportAPI.Data.Entities;

namespace SupportAPI.Auth
{
    public class AuthDbSeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthDbSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }

        private async Task AddAdminUser()
        {
            var newAdminUser = new AppUser()
            {
                UserName = "akuzminas@bcbsm.com",
                Email = "akuzminas@bcbsm.com"
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if (existingAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "SafePassword123!");
                if(createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, AppRoles.All);
                }
            }
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in AppRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
