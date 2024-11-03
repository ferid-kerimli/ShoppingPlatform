using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class UserSettingsService : IUserSettingsService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSettingsService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
    {
        Log.Logger.Information(nameof(UpdatePassword) + "Updating password for user");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not logged in with this email: {Email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, updatePasswordDto.CurrentPassword);
            
            if (!checkPassword)
            {
                response.Failure("Current password is incorrect", 400);
                Log.Logger.Warning(nameof(UpdatePassword) + "Current password is incorrect for user: {Email}", userEmail);
                return response;
            }
            
            var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
            
            if (result.Succeeded)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(UpdatePassword) + "Password updated successfully for user: {Email}", userEmail);
            }
            else
            {
                response.Failure("Password update failed", 400);
                Log.Logger.Warning(nameof(UpdatePassword) + "Password update failed for user: {Email}", userEmail);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(UpdatePassword) + "Error while updating password for user: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> ChangeUsername(ChangeUsernameDto changeUsernameDto)
    {
        Log.Logger.Information(nameof(ChangeUsername) + "Changing username for user");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not logged in with this email: {Email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not found with this email: {email}", userEmail);
                return response;
            }

            user.UserName = changeUsernameDto.NewUsername;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(ChangeUsername) + "Username changed successfully for user: {Email}", userEmail);
            }
            else
            {
                response.Failure("Username change failed", 400);
                Log.Logger.Warning(nameof(ChangeUsername) + "Username change failed for user: {Email}", userEmail);
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(ChangeUsername) + " - Error while changing username for user: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> ChangeEmail(ChangeEmailDto changeEmailDto)
    {
        Log.Logger.Information(nameof(ChangeEmail) + "Changing email for user");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not logged in with this email: {Email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(UpdatePassword) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, changeEmailDto.NewEmail);
            var result = await _userManager.ChangeEmailAsync(user, changeEmailDto.NewEmail, token);

            if (result.Succeeded)
            {
                await _userManager.UpdateAsync(user);

                response.Success(true, 200);
                Log.Logger.Information(nameof(ChangeEmail) + "Email changed successfully for user: {CurrentEmail}", userEmail);
            }
            else
            {
                response.Failure("Email change failed", 400);
                Log.Logger.Warning(nameof(ChangeEmail) + "Email change failed for user: {CurrentEmail}", userEmail);
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(ChangeEmail) + "Error while changing email for user: {Message}", e.Message);
            throw;
        }

        return response;
    }
}