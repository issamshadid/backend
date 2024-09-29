using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Template.Contracts.Attribute;

namespace Template.API.ErrorHandling;

/// <summary>
///     Hybrid Model Binder
/// </summary>
public class HybridModelBinder : IModelBinder
{
    private static readonly Type HybridFromRouteType = typeof(HybridFromRouteAttribute);
    private readonly BodyModelBinder _defaultBinder;

    /// <summary>
    ///     Hybrid Model Binder
    /// </summary>
    public HybridModelBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
    {
        _defaultBinder = new BodyModelBinder(formatters, readerFactory);
    }

    /// <summary>
    ///     Model Binder
    /// </summary>
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        await _defaultBinder.BindModelAsync(bindingContext);
        if (bindingContext.Result.IsModelSet)
        {
            if (bindingContext.Result.Model is not object model)
                return;

            var modelType = model.GetType();

            var properties = model.GetType().GetProperties()
                .Where(property => property.CustomAttributes.Any(HaveRouteParamMatching));

            foreach (var kvp in properties
                         .Select(prop => new KeyValuePair<string, PropertyInfo>(GetRouteParamName(prop), prop))
                         .Where(kvp => bindingContext.ValueProvider.GetValue(kvp.Key).Any()))
            {
                if (modelType.GetProperty(kvp.Value.Name) is not PropertyInfo propertyInfo)
                    continue;

                var value = bindingContext.ValueProvider.GetValue(kvp.Key).FirstValue;
                propertyInfo.SetValue(model, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            }

            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }

    private static bool HaveRouteParamMatching(CustomAttributeData attr)
    {
        return HybridFromRouteType.IsAssignableFrom(attr.AttributeType);
    }

    private static string GetRouteParamName(PropertyInfo propertyInfo)
    {
        var attribute =
            propertyInfo.GetCustomAttributes(HybridFromRouteType, false).FirstOrDefault() as
                HybridFromRouteAttribute;
        return attribute?.RouteParameterName ?? propertyInfo.Name;
    }
}