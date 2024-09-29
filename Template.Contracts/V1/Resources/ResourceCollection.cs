using Template.Contracts.V1.Models;

namespace Template.Contracts.V1.Resources;

/// <summary>
///     Collection of items with metadata.
/// </summary>
/// <typeparam name="TResource">Items type.</typeparam>
public class ResourceCollection<TResource>
{
    /// <summary>
    ///     Retrieves an empty list
    /// </summary>
    public ResourceCollection()
    {
        Items = Enumerable.Empty<TResource>();
    }

    /// <summary>
    ///     Retrieves the list.
    /// </summary>
    /// <param name="items">Items to be returned</param>
    public ResourceCollection(IEnumerable<TResource> items)
    {
        Items = items;
    }

    /// <summary>
    ///     Retrieves the list and counts the items
    /// </summary>
    /// <param name="items">Items to be returned</param>
    /// <param name="totalResults">Number of items in list</param>
    public ResourceCollection(
        IEnumerable<TResource> items,
        long totalResults)
    {
        Items = items;
        TotalResults = totalResults;
    }

    /// <summary>
    ///     Retrieves the list, counts the items and the elapsed time
    /// </summary>
    /// <param name="items">Items to be returned</param>
    /// <param name="totalResults">Number of items in list</param>
    /// <param name="elapsedMilliseconds">Elapsed time in milliseconds</param>
    public ResourceCollection(
        IEnumerable<TResource> items,
        long totalResults,
        long elapsedMilliseconds)
    {
        Items = items;
        TotalResults = totalResults;
        ElapsedMilliseconds = elapsedMilliseconds;
    }

    /// <summary>
    ///     Retrieves the list, counts the items and the elapsed time
    /// </summary>
    /// <param name="items">Items to be returned</param>
    /// <param name="totalResults">Number of items in list</param>
    /// <param name="filter">Filter to get skip and take properties</param>
    public ResourceCollection(
        IEnumerable<TResource> items,
        long totalResults,
        ListFilter? filter)
    {
        Items = items;
        TotalResults = totalResults;
        Skip = filter?.Skip;
        Take = filter?.Take;
    }

    public ResourceCollection(
        IEnumerable<TResource> items,
        long totalResults,
        long elapsedMilliseconds,
        int? skip,
        int? take)
    {
        Items = items;
        TotalResults = totalResults;
        ElapsedMilliseconds = elapsedMilliseconds;
        Skip = skip;
        Take = take;
    }

    /// <summary>
    ///     Elapsed time of the query.
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    ///     Collection of items.
    /// </summary>
    public IEnumerable<TResource> Items { get; set; }

    /// <summary>
    ///     Total items count.
    /// </summary>
    /// <remarks>Useful for paging.</remarks>
    public long TotalResults { get; set; }

    /// <summary>
    ///     Skip a number of records from the top of the list
    /// </summary>
    /// <remarks>Useful for paging.</remarks>
    public int? Skip { get; set; }

    /// <summary>
    ///     Take only a specific number of records from the list
    /// </summary>
    /// <remarks>Useful for paging.</remarks>
    public int? Take { get; set; }
}