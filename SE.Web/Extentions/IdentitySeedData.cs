using Microsoft.AspNetCore.Identity;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Extentions
{
    public static class IdentitySeedData
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers (UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("izmiregitimkurumlari@gmail.com").Result == null)
            {
                User user = new User();
                user.UserName = "erguntoprak";
                user.FirsName = "Ergün";
                user.LastName = "Toprak";
                user.Email = "izmiregitimkurumlari@gmail.com";
                user.EmailConfirmed = true;
                user.IsActive = true;
                user.PhoneNumber = "5543187924";
                IdentityResult result = userManager.CreateAsync
                (user, "erSap*24").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        public static void SeedRoles (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
