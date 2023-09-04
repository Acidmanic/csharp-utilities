using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acidmanic.Utilities.Extensions;

namespace Acidmanic.Utilities.Filtering
{
    public class FilterQuery
    {
        private readonly Dictionary<string, FilterItem> _itemsByKey = new Dictionary<string, FilterItem>();

        public Type EntityType { get; set; } = typeof(object);

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

            sb.Append(EntityType.FullName);
            
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


        public override string ToString()
        {
            Func<string, string> cover = s => s;

            if (_itemsByKey.Count > 1)
            {
                cover = s => "(" + s + ")";
            }

            var itemStrings = _itemsByKey.Values.Select(i => i.ToString()).Select(cover);

            return string.Join(" & ", itemStrings);
        }
    }
}