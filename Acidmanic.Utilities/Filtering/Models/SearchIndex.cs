using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Models
{

    public class SearchIndex<TId>
    {
        
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        public TId ResultId { get; set; }

        public string IndexCorpus { get; set; }


    }
}