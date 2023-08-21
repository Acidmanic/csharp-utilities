using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Models
{
    public class FilterResult
    {
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        /// <summary>
        /// This field should be set by filter-query's hash 
        /// </summary>
        public string FilterHash { get; set; }

        public long ResultId { get; set; }

        /// <summary>
        /// This field should be set when the filter is performed
        /// </summary>
        public long ExpirationTimeStamp { get; set; }
    }
}