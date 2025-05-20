using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_SOS_Code.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "2",
                AccessFailedCount = 0,
                ConcurrencyStamp = "16df9172-907f-4d70-8b78-188aff7964a3",
                EmailConfirmed = false,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEOS7WlWvOnI1LzqpwlC2jel9+yE2OBzJmmMth/5fyc8fGXVLd+Xi7Cadm1ewYtYufw==",
                PhoneNumberConfirmed = false,
                SecurityStamp = "b36c88d9-a671-479e-91d9-9b653fb67b8f",
                TwoFactorEnabled = false,
                UserName = "TicketUser"
            });
        }
    }
}
