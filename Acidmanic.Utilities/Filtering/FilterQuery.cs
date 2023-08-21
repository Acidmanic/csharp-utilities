using System.Collections.Generic;
using System.Text;
using Acidmanic.Utilities.Extensions;

namespace Acidmanic.Utilities.Filtering
{
    public class FilterQuery
    {
        private readonly Dictionary<string, FilterItem> _itemsByKey = new Dictionary<string, FilterItem>();

        /// <summary>
        /// This Property is being used in hash generation, there fore you can use it to have filters with different hashes
        /// for situations you might need to have different hashes from a same filter, like making filters distinguished
        /// regarding the entity type.  
        /// </summary>
        public string FilterName { get; set; } = "Filter";

        public void Add(FilterItem item)
        {
            var key = item.Key?.ToLower();

            _itemsByKey.Add(key!, item);
        }

        public void Clear()
        {
            _itemsByKey.Clear();
        }

        public FilterItem this[string key]
        {
            get
            {
                key = key?.ToLower();

                return _itemsByKey[key!];
            }
        }

        public List<FilterItem> Items()
        {
            var list = new List<FilterItem>();

            list.AddRange(_itemsByKey.Values);

            return list;
        }

        public List<string> NormalizedKeys()
        {
            var list = new List<string>();

            list.AddRange(_itemsByKey.Keys);

            return list;
        }

        public string Hash()
        {
            var sb = new StringBuilder();

            sb.Append(FilterName);
            
            var sep = "";

            var keys = NormalizedKeys();

            keys.Sort();

            foreach (var key in keys)
            {
                var item = _itemsByKey[key];

                sb.Append(sep).Append(item.ToColumnSeparatedString());
                sep = ":";
            }

            return sb.ToString().ComputeMd5();
        }
    }
}