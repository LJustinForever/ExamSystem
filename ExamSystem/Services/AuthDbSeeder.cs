using ExamSystem.Model;
using Microsoft.AspNetCore.Identity;

namespace ExamSystem.Services;

public class AuthDbSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    public AuthDbSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await AddDefaultRoles();
        await AddAdminUser();
    }

    private async Task AddDefaultRoles()
    {
        foreach (var role in UserRoles.ALL)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    private async Task AddAdminUser()
    {
        var newAdminUser = new ApplicationUser()
        {
            UserName = "admin",
            Email = "admin@admin.com",
            Name = "adminas",
            LastName = "adminauskas",
        };
        var exsitingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
        if (exsitingAdminUser == null)
        {
            var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "Password123.");
            if (createAdminUserResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(newAdminUser, UserRoles.ALL);
            }
        }
    }
}
