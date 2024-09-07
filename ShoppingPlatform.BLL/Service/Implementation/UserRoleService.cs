using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class UserRoleService : IUserRoleService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UserRoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<bool>> AssignRoleToUser(int userId, string roleName)
    {
        Log.Logger.Information(nameof(AssignRoleToUser) + "AssignRoleToUser called");
        var response = new ApiResponse<bool>();

        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(AssignRoleToUser) + "User not found with this id: {id}", userId);
                return response;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                response.Failure("Role does not exist", 404);
                Log.Logger.Warning(nameof(AssignRoleToUser) + "Role does not exist: {roleName}", roleName);
                return response;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(AssignRoleToUser) + "AssignRoleToUser completed successfully for user: {user}", user);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                response.Failure($"Failed to assign role to user: {errorMessage}", 400);
                Log.Logger.Error(nameof(AssignRoleToUser) + "Failed to assign role to user: {errorMessage}", errorMessage);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(AssignRoleToUser) + "Error happened while assigning role to user: {Message}", e.Message);
            throw;
        }

        return response;
    }
}