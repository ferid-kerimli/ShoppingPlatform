using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IUserRoleService _userRoleService;

    public AdminController(IUserService userService, IRoleService roleService, IUserRoleService userRoleService)
    {
        _userService = userService;
        _roleService = roleService;
        _userRoleService = userRoleService;
    }

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsers();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _userService.GetUserById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRoles();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        var result = await _roleService.GetRoleById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _roleService.CreateRole(roleName);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteRole")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var result = await _roleService.DeleteRole(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("AssignRoleToUser")]
    public async Task<IActionResult> AssignRoleToUser(int userId, string roleName)
    {
        var result = await _userRoleService.AssignRoleToUser(userId, roleName);
        return StatusCode(result.StatusCode, result);
    }
}