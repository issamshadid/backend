using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Business.Managers;
using Template.Contracts.V1.Filters;
using Template.Contracts.V1.Models;
using Template.Contracts.V1.Resources;
using Template.DataAccess;

namespace Template.API.V1.Controllers;

/// <summary>
///     Categories api endpoints.
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryWebManager _categoryWebManager;
    private readonly ILogger<CategoryController> _logger;

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="categoryWebManager"></param>
    public CategoryController(
        ILogger<CategoryController> logger,
        ICategoryWebManager categoryWebManager)
    {
        _logger = logger;
        _categoryWebManager = categoryWebManager;
    }

    /// <summary>
    ///     Get a list of all Categories.
    /// </summary>
    /// <remarks>Get a list of all Categories.</remarks>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpGet]
    [Route("", Name = "GetAllCategories")]
    [ProducesResponseType(typeof(ResourceCollection<CategoryResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllCategories([FromQuery] CategoryListFilter filter)
    {
        var chronometer = new Stopwatch().StartNow();
        var listOfResources = await _categoryWebManager.GetAllCategoriesAsync(filter);
        chronometer.Stop();
        var result = new ResourceCollection<CategoryResource>(listOfResources, listOfResources.Count,
            chronometer.ElapsedMilliseconds, filter.Skip, filter.Take);
        return Ok(result);
    }

    /// <summary>
    ///     Get a Category By Id.
    /// </summary>
    /// <param name="id">Id of the Category.</param>
    /// <remarks>Get a Category By Id.</remarks>
    /// <respone code="404">Category is not found.</respone>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:min(1)}", Name = "GetCategoryById")]
    [ProducesResponseType(typeof(CategoryResource), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var resource = await _categoryWebManager.GetCategoryByIdAsync(id);
        return Ok(resource);
    }

    /// <summary>
    ///     Create a new Category.
    /// </summary>
    /// <remarks>Create a new Category.</remarks>
    /// <param name="model">The Category model.</param>
    /// <response code="400">The model is not valid.</response>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpPost]
    [Route("", Name = "CreateCategory")]
    [ProducesResponseType(typeof(CategoryResource), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryModel model)
    {
        var resource = await _categoryWebManager.CreateCategoryAsync(model);
        return CreatedAtAction
        (
            nameof(GetCategoryById),
            new
            {
                id = resource.Id
            },
            resource);
    }

    /// <summary>
    ///     Update an existing Category.
    /// </summary>
    /// <remarks>Update an existing Category.</remarks>
    /// <param name="id">The Category Id.</param>
    /// <param name="model">The Category model.</param>
    /// <response code="400">The model is not valid.</response>
    /// <response code="404">Category is not found.</response>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:min(1)}", Name = "UpdateCategory")]
    [ProducesResponseType(typeof(CategoryResource), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateCategory(
        int id, [FromBody] CategoryModel model)
    {
        var resource = await _categoryWebManager.UpdateCategoryAsync(id, model);
        return Ok(resource);
    }

    /// <summary>
    ///     Delete a Category.
    /// </summary>
    /// <remarks>Delete a Category.</remarks>
    /// <response code="204">No Content.</response>
    /// <response code="404">Category is not found.</response>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id:min(1)}", Name = "DeleteCategory")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryWebManager.DeleteCategoryAsync(id);
        return NoContent();
    }
}