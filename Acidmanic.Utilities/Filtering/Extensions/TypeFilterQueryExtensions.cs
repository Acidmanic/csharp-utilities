using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Acidmanic.Utilities.DataTypes;
using Acidmanic.Utilities.Filtering.Attributes;
using Acidmanic.Utilities.Reflection.ObjectTree;
using Acidmanic.Utilities.Reflection.ObjectTree.FieldAddressing;
using Acidmanic.Utilities.Results;

namespace Acidmanic.Utilities.Filtering.Extensions
{
    public static class TypeFilterQueryExtensions
    {
        private static readonly long SecondMilliseconds = 1000;
        private static readonly long MinuteMilliseconds = 60 * SecondMilliseconds;
        private static readonly long HourMilliseconds = 60 * MinuteMilliseconds;
        private static readonly long DayMilliseconds = 24 * HourMilliseconds;


        public static long GetFilterResultExpirationDurationMilliseconds(this Type type)
        {
            var attribute = type.GetCustomAttributes<FilterResultExpirationDurationAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                return attribute.Milliseconds;
            }

            return 12 * HourMilliseconds;
        }


        public static TimeSpan GetFilterResultExpirationDurationTimeSpan(this Type type)
        {
            var totalMilliseconds = GetFilterResultExpirationDurationMilliseconds(type);

            int days = (int)(totalMilliseconds / DayMilliseconds);

            totalMilliseconds %= DayMilliseconds;

            int hours = (int)(totalMilliseconds / HourMilliseconds);

            totalMilliseconds %= HourMilliseconds;

            int minutes = (int)(totalMilliseconds / MinuteMilliseconds);

            totalMilliseconds %= MinuteMilliseconds;

            int seconds = (int)(totalMilliseconds / SecondMilliseconds);

            totalMilliseconds %= SecondMilliseconds;

            return new TimeSpan(days, hours, minutes, seconds, (int)totalMilliseconds);
        }

        public static long GetFilterResultExpirationPointMilliseconds(this Type type)
        {
            return TimeStamp.Now.TotalMilliSeconds + GetFilterResultExpirationDurationMilliseconds(type);
        }

        public static List<FieldKey> GetFilterFields(this Type type)
        {
            var evaluator = new ObjectEvaluator(type);

            var filterKeys = evaluator.Map
                .Nodes.Where(n => n.PropertyAttributes.Any(att => att is FilterFieldAttribute))
                .Select(n => evaluator.Map.FieldKeyByNode(n)).ToList();

            return filterKeys;
        }


        public static FilterQuery GetFilterQuery(this Type type, bool fullTree = false,
            params Dictionary<string, ICollection<string>>[] queries)
        {
            var evaluator = new ObjectEvaluator(type);

            var leaves = (fullTree ? evaluator.Map.Nodes : evaluator.RootNode.GetDirectLeaves())
                .Where(n => n.IsLeaf)
                .Where(n => n.PropertyAttributes.Any(att => att is FilterFieldAttribute));

            var query = new FilterQuery
            {
                EntityType = type
            };

            foreach (var leaf in leaves)
            {
                var address = evaluator.Map.FieldKeyByNode(leaf)
                    .Headless().ToString();

                var foundKey = FindKey(address, queries);

                if (foundKey)
                {
                    var item = new FilterItem();

                    foreach (var value in foundKey.Value)
                    {
                        ApplyQueryItemOnFilterItem(item, value);
                    }

                    item.Key = address;
                    item.ValueType = leaf.Type;

                    query.Add(item);
                }
            }

            return query;
        }


        private static Result<ICollection<string>> FindKey(string address,
            Dictionary<string, ICollection<string>>[] queries)
        {
            var lowerAddress = address.ToLower();

            foreach (var dictionary in queries)
            {
                foreach (var key in dictionary.Keys)
                {
                    if (key.ToLower() == lowerAddress)
                    {
                        return new Result<ICollection<string>>(true, dictionary[key]);
                    }
                }
            }

            return new Result<ICollection<string>>().FailAndDefaultValue();
        }

        private static void ApplyQueryItemOnFilterItem(FilterItem item, string queryValue)
        {
            if (queryValue.StartsWith("<"))
            {
                item.Maximum = queryValue.Substring(1, queryValue.Length - 1);
                item.ValueComparison = ValueComparison.SmallerThan;
                return;
            }

            if (queryValue.StartsWith(">"))
            {
                item.Minimum = queryValue.Substring(1, queryValue.Length - 1);
                item.ValueComparison = ValueComparison.LargerThan;
                return;
            }

            if (queryValue.Contains("<>", StringComparison.Ordinal))
            {
                var segments = queryValue.Split("<>", StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length > 1)
                {
                    item.Minimum = segments[0];
                    item.Maximum = segments[1];
                    item.ValueComparison = ValueComparison.BetweenValues;
                    return;
                }
            }

            item.EqualValues.Add(queryValue);
            item.ValueComparison = ValueComparison.Equal;
        }
    }
}