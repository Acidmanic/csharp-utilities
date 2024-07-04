using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Models
{

    public class SearchIndex<TId>
    {
        
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        public virtual TId ResultId { get; set; }

        public string IndexCorpus { get; set; }
    }

    internal class SearchIndexForceLeafId<TId> : SearchIndex<TId>
    {
        [TreatAsLeaf]
        public override TId ResultId { get; set; }
    }
}