using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Models
{
    /// <summary>
    /// This entity stores the result of a search/filtering operations for another entity
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class FilterResult<TId>
    {
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        /// <summary>
        /// This field holds the unique id for each specific search (filtering)
        /// in case that client code needs to
        /// trace filter results per search. 
        /// </summary>
        public string SearchId { get; set; }
        /// <summary>
        /// This filed would store the id of found entity
        /// </summary>
        public virtual TId ResultId { get; set; }

        /// <summary>
        /// This field should be set when the filter is performed
        /// </summary>
        public long ExpirationTimeStamp { get; set; }
    }

    internal class FilterResultForceLeafId<TId> : FilterResult<TId>
    {
        [TreatAsLeaf]
        public override TId ResultId { get; set; }
    }
}