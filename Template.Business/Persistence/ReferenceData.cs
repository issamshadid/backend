using System.Text;
using Autofac;
using Newtonsoft.Json;
using Template.Contracts.V1.Filters;
using Template.DataAccess.Entities;
using Template.DataAccess.Repositories;

namespace Template.Business.Persistence;

public static class ReferenceData
{
    public static async Task CreateAsync(
        ILifetimeScope container)
    {
        var categoryRepository = container.Resolve<ICategoryRepository>();

        await CreateDefaultCategories(categoryRepository);
    }

    private static async Task CreateDefaultCategories(ICategoryRepository categoryRepository)
    {
        var categories = await categoryRepository.LoadCategoriesAsync(new CategoryListFilter());
        categories = categories.Where(x => !x.IsDeleted).ToList();
        var dataSet = LoadCategoryDataSet();
        foreach (var data in dataSet)
        {
            var entity = categories.Find(x => x.Name == data.Name);
            if (entity == null)
            {
                await categoryRepository.CreateCategoryAsync(new CategoryEntity
                {
                    Name = data.Name,
                    OtherName = data.OtherName,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CreatedBy = "System"
                });
            }
            else if (!CompareTypes(entity, data))
            {
                entity.Name = data.Name;
                entity.OtherName = data.OtherName;
                await categoryRepository.UpdateCategoryAsync(entity);
            }
        }
    }

    #region private Methods

    private static bool CompareTypes(CategoryEntity firstType, JsonCategory secondType)
    {
        return firstType.Name == secondType.Name && firstType.OtherName == secondType.OtherName;
    }

    private static List<JsonCategory> LoadCategoryDataSet()
    {
        var json = ReadJsonFileResource("category.json");
        return JsonConvert.DeserializeObject<List<JsonCategory>>(json)!;
    }

    private sealed class JsonCategory
    {
        public string Name { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
    }

    private static string ReadJsonFileResource(
        string fileName)
    {
        var resourceName = "Template.Business.Persistence.Data." + fileName;

        var assembly = typeof(ReferenceData).Assembly;
        var resourceStream = assembly.GetManifestResourceStream(resourceName);
        if (resourceStream == null) return string.Empty;
        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    #endregion
}