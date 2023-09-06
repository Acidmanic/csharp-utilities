using System;
using Acidmanic.Utilities.Filtering.Models;

namespace Acidmanic.Utilities.Filtering.Extensions
{
    public static class FilterResultSearchIdExtensions
    {
        /// <summary>
        /// This method sets a new guid value without hyphens as filter result instance's SearchId.
        /// </summary>
        /// <returns>The given filter result instance with it's search-id set.</returns>
        /// <param name="filterResult"></param>
        public static FilterResult<TId> NewSearchId<TId>(this FilterResult<TId> filterResult)
        {
            var guid = Guid.NewGuid();

            filterResult.SearchId = guid.ToString("N");

            return filterResult;
        }
    }
}