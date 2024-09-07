using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoppingPlatform.BLL.Dto.RoleDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<RoleGetDto>>> GetAllRoles()
    {
        Log.Logger.Information(nameof(GetAllRoles) + "GetAllRoles called");
        var response = new ApiResponse<List<RoleGetDto>>();

        try
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var mappedRoles = _mapper.Map<List<RoleGetDto>>(roles);
            
            response.Success(mappedRoles, 200);
            Log.Logger.Information(nameof(GetAllRoles) + "GetAllRoles completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetAllRoles) + "Error happened while getting all roles: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<RoleGetDto>> GetRoleById(int id)
    {
        Log.Logger.Information(nameof(GetRoleById) + "GetRoleById called");
        var response = new ApiResponse<RoleGetDto>();

        try
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                response.Failure("Role not found", 404);
                Log.Logger.Warning(nameof(GetRoleById) + "Role not found with this id: {id}", id);
                return response;
            }

            var mappedRole = _mapper.Map<RoleGetDto>(role);
            
            response.Success(mappedRole, 200);
            Log.Logger.Information(nameof(GetRoleById) + "GetRoleById completed succesfully for this id: {id}", id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetRoleById) + "Error happened while getting role by id: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> CreateRole(string roleName)
    {
        Log.Logger.Information(nameof(CreateRole) + "CreateRole Called");
        var response = new ApiResponse<bool>();

        try
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                response.Failure("Please insert role name", 400);
                Log.Logger.Warning(nameof(CreateRole) + "role name is empty");
                return response;
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                response.Failure("Role already exists", 400);
                Log.Logger.Warning(nameof(CreateRole) + "Role already exists with this name: {roleName}", roleName);
                return response;
            }

            var newRole = new AppRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                response.Success(true, 201);
                Log.Logger.Information(nameof(CreateRole) + "Role Created successfully: {roleName}", roleName);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                response.Failure($"Failed to create role: {errorMessage}", 400);
                Log.Logger.Warning(nameof(DeleteRole) + "Failed to create role {errorMessage})", errorMessage);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(CreateRole) + "Error happened while creating role: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> DeleteRole(int id)
    {
        Log.Logger.Information(nameof(DeleteRole) + "DeleteRole called");
        var response = new ApiResponse<bool>();

        try
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                response.Failure("Role not found", 404);
                Log.Logger.Warning(nameof(DeleteRole) + "Role not found with this id: {id}", id);
                return response;
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(DeleteRole) + "DeleteRole completed successfully for this id: {id}", id);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                response.Failure($"Failed to delete role: {errorMessage}", 400);
                Log.Logger.Error(nameof(DeleteRole) + "Failed to delete role: {errorMessage}", errorMessage);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(DeleteRole) + "Error happened while deleting role: {Message}", e.Message);
            throw;
        }

        return response;
    }
}