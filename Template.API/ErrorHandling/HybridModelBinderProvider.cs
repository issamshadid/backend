using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Template.Contracts.Attribute;

namespace Template.API.ErrorHandling;

/// <summary>
///     Hybrid Model Binder Provider
/// </summary>
public class HybridModelBinderProvider : IModelBinderProvider
{
    private readonly IList<IInputFormatter> _formatters;
    private readonly IHttpRequestStreamReaderFactory _readerFactory;

    /// <summary>
    ///     Hybrid Model Binder Provider
    /// </summary>
    /// <param name="formatters"></param>
    /// <param name="readerFactory"></param>
    public HybridModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
    {
        _formatters = formatters;
        _readerFactory = readerFactory;
    }

    /// <summary>
    ///     Get Binder
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.IsComplexType && context.Metadata.ModelType.IsDefined(typeof(HybridModelAttribute), false))
            return new HybridModelBinder(_formatters, _readerFactory);

        return null;
    }
}