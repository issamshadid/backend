using Template.Business.Mappings;
using Template.Configurations.ExceptionHandler;
using Template.Contracts;
using Template.Contracts.V1.Filters;
using Template.Contracts.V1.Models;
using Template.Contracts.V1.Resources;
using Template.DataAccess;
using Template.DataAccess.Entities;
using Template.DataAccess.Repositories;

namespace Template.Business.Managers;

public interface ICategoryWebManager
{
    #region Web Logic

    Task<CategoryResource> GetCategoryByIdAsync(int id);
    Task<List<CategoryResource>> GetAllCategoriesAsync(CategoryListFilter filter);
    Task<CategoryResource> CreateCategoryAsync(CategoryModel model);
    Task<CategoryResource> UpdateCategoryAsync(int id, CategoryModel model);
    Task DeleteCategoryAsync(int id);

    #endregion
}

public class CategoryManager : ICategoryWebManager
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryManager(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    #region Web Logic

    public async Task<CategoryResource> GetCategoryByIdAsync(int id)
    {
        var entity = await _categoryRepository.LoadCategoryByIdAsync(id);
        CheckIfEntityNotExists(entity, id);

        return entity.MapEntityToResource()!;
    }

    public async Task<List<CategoryResource>> GetAllCategoriesAsync(CategoryListFilter filter)
    {
        var entities = (await _categoryRepository.LoadCategoriesAsync(filter)).Where(x => !x.IsDeleted);

        return entities.Select(c => c.MapEntityToResource()!).ToList();
    }

    public async Task<CategoryResource> CreateCategoryAsync(CategoryModel model)
    {
        await ValidateCategory(model);

        var entity = model.MapModelToEntity();
        await _categoryRepository.CreateCategoryAsync(entity!);

        var createdEntity = entity.MapEntityToResource();

        return createdEntity!;
    }

    public async Task<CategoryResource> UpdateCategoryAsync(int id, CategoryModel model)
    {
        await ValidateCategory(model, id);
        var entity = await _categoryRepository.LoadCategoryByIdAsync(id);
        CheckIfEntityNotExists(entity, id);

        entity = model.MapModelToEntity(id)!.WithTrackableInfoCreate(entity!);
        await _categoryRepository.UpdateCategoryAsync(entity!);

        var createdEntity = entity.MapEntityToResource();

        return createdEntity!;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var entity = await _categoryRepository.LoadCategoryByIdAsync(id);
        CheckIfEntityNotExists(entity, id);

        await _categoryRepository.DeleteCategory(entity!);
    }

    #endregion

    #region Privatr Methods

    private static void CheckIfEntityNotExists(CategoryEntity? entity, int id)
    {
        if (entity == null || entity.IsDeleted)
            ExceptionManager.ThrowItemNotFoundException("Id",
                ValidationMessages.GetNotFoundMessage("Category", id.ToString()));
    }

    private async Task ValidateCategory(CategoryModel model, int? id = null)
    {
        var isCategoryExists = id == null
            ? await _categoryRepository.ExistsAsync(x => x.Name == model.Name && !x.IsDeleted)
            : await _categoryRepository.ExistsAsync(x => x.Name == model.Name && x.Id != id && !x.IsDeleted);

        if (isCategoryExists)
            ExceptionManager.ThrowConflictException(nameof(model.Name),
                ValidationMessages.GetAlreadyExistsMessage(nameof(model.Name).ToLower(), model.Name));

        if (string.IsNullOrEmpty(model.Name))
            ExceptionManager.ThrowInvalidModelException(nameof(model.Name),
                ValidationMessages.GetNotEmptyMessage(nameof(model.Name)));

        if (string.IsNullOrEmpty(model.OtherName))
            ExceptionManager.ThrowInvalidModelException(nameof(model.OtherName),
                ValidationMessages.GetNotEmptyMessage(nameof(model.OtherName)));
    }

    #endregion
}