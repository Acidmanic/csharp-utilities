using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Models
{
    public class FilterResult
    {
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        /// <summary>
        /// This field holds the unique id for each specific search (filtering)
        /// in case that client code needs to
        /// trace filter results per search. 
        /// </summary>
        public string SearchId { get; set; }

        public long ResultId { get; set; }

        /// <summary>
        /// This field should be set when the filter is performed
        /// </summary>
        public long ExpirationTimeStamp { get; set; }
    }
}