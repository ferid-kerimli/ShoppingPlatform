using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder builder)
    {
        var role = new AppRole
        {
            Id = 1,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        
        var user = new AppUser()
        {
            Id = 1,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, "admin123!"),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        builder.Entity<AppRole>().HasData(role);
        builder.Entity<AppUser>().HasData(user);
        
        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int>
            {
                UserId = user.Id,
                RoleId = role.Id
            }
        );
    }
}