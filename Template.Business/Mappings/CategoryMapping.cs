using Template.Contracts.V1.Models;
using Template.Contracts.V1.Resources;
using Template.DataAccess.Entities;

namespace Template.Business.Mappings;

public static class CategoryMapping
{
    public static CategoryResource? MapEntityToResource(
        this CategoryEntity? categoryEntity)
    {
        if (categoryEntity == null) return null;
        var categoryResource = new CategoryResource
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name,
            OtherName = categoryEntity.OtherName
        };

        return categoryResource;
    }

    public static CategoryEntity? MapModelToEntity(
        this CategoryModel? categoryModel,
        int id = 0)
    {
        if (categoryModel == null) return null;
        var categoryEntity = new CategoryEntity
        {
            Id = id,
            Name = categoryModel.Name,
            OtherName = categoryModel.OtherName
        };

        return categoryEntity;
    }
}