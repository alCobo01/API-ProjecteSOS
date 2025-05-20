using Microsoft.AspNetCore.Identity;

namespace API_SOS_Code.Tools
{
    public static class RoleTools
    {
        public static async Task CreateInitialRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] rols = { "Admin", "User" };

            foreach (var rol in rols)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }
    }
}
