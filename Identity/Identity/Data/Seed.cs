using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Identity.API.Entities;

namespace Identity.API.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {

            try
            {
                if (!userManager.Users.Any())
                {
                    var users = new List<AppUser> {
                    new AppUser {
                        Id = "a",
                        UserName = "NigelDewar",
                        Email = "nigeldewar@live.com",
                        EmailConfirmed = true
                    },
                    new AppUser {
                        Id = "b",
                        UserName = "Tom.Jones",
                        Email = "tom.jones@supergoodrealty465465.com.au",
                        EmailConfirmed = true
                    },
                    new AppUser {
                        Id = "c",
                        UserName = "Care-Bare_29",
                        Email = "jane464654654654@doobarsdfds.com",
                        EmailConfirmed = true
                    }
                };
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "Password1!");
                    }
                }
                
                if (roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult() == false)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (roleManager.RoleExistsAsync("User").GetAwaiter().GetResult() == false)
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }
                
                var admin = userManager.FindByEmailAsync("nigeldewar@live.com").GetAwaiter().GetResult();
                
                if (userManager.IsInRoleAsync(admin, "Admin").GetAwaiter().GetResult() == false)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

         
            }
            catch (Exception ex)
            {

                throw ex;
            }

            

        }
    }
}