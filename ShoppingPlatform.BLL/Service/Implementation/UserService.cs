using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<UserGetDto>>> GetAllUsers()
    {
        Log.Logger.Information(nameof(GetAllUsers) + " Getting all users started");
        var response = new ApiResponse<List<UserGetDto>>();

        try
        {
            var users = await _userManager.Users.ToListAsync();

            var mappedUsers = _mapper.Map<List<UserGetDto>>(users);
            
            response.Success(mappedUsers, 200);
            Log.Logger.Information(nameof(GetAllUsers) + " Getting all users completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetAllUsers) + " Error happened while getting all users {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<UserGetDto>> GetUserById(int id)
    {
        Log.Logger.Information(nameof(GetUserById) + " Getting user with {id} id started", id);
        var response = new ApiResponse<UserGetDto>();

        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                response.Failure("User not found");
                Log.Logger.Warning(nameof(GetUserById) + "User not found with this id: {id}", id);
                return response;
            }

            var mappedUser = _mapper.Map<UserGetDto>(user);
            
            response.Success(mappedUser, 200);
            Log.Logger.Information(nameof(GetUserById) + "Getting user with this id completed successfully: {id}", id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetUserById) + " Error happened while getting user: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> DeleteUser(int id)
    {
        Log.Logger.Information(nameof(DeleteUser) + " Delete {id} user started", id);
        var response = new ApiResponse<bool>();

        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                response.Failure("User not found");
                Log.Logger.Warning(nameof(DeleteUser) + " User not found with id: {id}", id);
                return response;
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(DeleteUser) + " User deleted successfully");
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                response.Failure($"Failed to delete user: {errorMessage}", 400);
                Log.Logger.Error(nameof(DeleteUser) + " User can not be deleted with this id: {ErrorMessage}", errorMessage);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(DeleteUser) + " Error happened while deleting user: {Message}", e.Message);
            throw;
        }

        return response;
    }
}