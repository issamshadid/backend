using System.Linq.Expressions;
using Template.Contracts.V1.Filters;
using Template.Contracts.V1.Resources;
using Template.DataAccess.Entities;

namespace Template.DataAccess.Repositories;

public interface ICategoryRepository
{
    Task<List<CategoryEntity>> LoadCategoriesAsync(CategoryListFilter filter);

    Task<CategoryEntity?> LoadCategoryByIdAsync(int id);

    Task CreateCategoryAsync(CategoryEntity entity);

    Task UpdateCategoryAsync(CategoryEntity entity);

    Task DeleteCategory(CategoryEntity entity);

    Task<bool> ExistsAsync(Expression<Func<CategoryEntity, bool>> filters);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IRepository<CategoryEntity, int> _categoryRepository;

    public CategoryRepository(
        IRepository<CategoryEntity, int> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryEntity>> LoadCategoriesAsync(CategoryListFilter filter)
    {
        var filters = GetFilters(filter);
        var entities =
            await _categoryRepository.GetItemsAsync(filters, null, filter, typeof(CategoryResource), filter.OrderBy);

        return entities;
    }

    public async Task<CategoryEntity?> LoadCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task CreateCategoryAsync(CategoryEntity entity)
    {
        _categoryRepository.Add(entity);
        await _categoryRepository.SaveAsync();
    }

    public async Task UpdateCategoryAsync(CategoryEntity entity)
    {
        _categoryRepository.Update(entity);
        await _categoryRepository.SaveAsync();
    }

    public async Task DeleteCategory(CategoryEntity entity)
    {
        _categoryRepository.Delete(entity);
        await _categoryRepository.SaveAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<CategoryEntity, bool>> filters)
    {
        return await _categoryRepository.ExistsAsync(new[] { filters });
    }


    #region PrivateMethods

    private static Expression<Func<CategoryEntity, bool>>[] GetFilters(
        CategoryListFilter filter)
    {
        var filters = new List<Expression<Func<CategoryEntity, bool>>>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            filters.Add(e => e.Name.Contains(filter.Name) || e.OtherName.Contains(filter.Name));

        return filters.ToArray();
    }

    #endregion
}