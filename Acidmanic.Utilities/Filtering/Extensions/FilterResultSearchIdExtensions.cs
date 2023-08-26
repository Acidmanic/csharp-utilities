using System;
using Acidmanic.Utilities.Filtering.Models;

namespace Acidmanic.Utilities.Filtering.Extensions
{
    public static class FilterResultSearchIdExtensions
    {


        /// <summary>
        /// This method sets a new guid value without hyphens as filter result instance's SearchId
        /// </summary>
        /// <returns>The given filter result instance with it's search-id set.</returns>
        /// <param name="filterResult"></param>
        public static FilterResult NewSearchId(this FilterResult filterResult)
        {
            filterResult.SearchId = Guid.NewGuid().ToString("N");

            return filterResult;
        }
    }
}