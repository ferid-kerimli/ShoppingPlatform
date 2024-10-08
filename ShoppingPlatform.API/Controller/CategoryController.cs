﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.CategoryDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategories();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetCategoryById")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _categoryService.GetCategoryById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var result = await _categoryService.CreateCategory(categoryCreateDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
    {
        var result = await _categoryService.UpdateCategory(categoryUpdateDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategory(id);
        return StatusCode(result.StatusCode, result);
    }
}