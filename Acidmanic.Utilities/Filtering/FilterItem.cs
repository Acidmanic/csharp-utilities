using System;
using System.Collections.Generic;

namespace Acidmanic.Utilities.Filtering
{
    public class FilterItem
    {
        private static readonly string[] OutOfHashStrings = { ":" };
        public string Key { get; set; }

        public object Maximum { get; set; }

        public object Minimum { get; set; }

        public Type ValueType { get; set; } = typeof(string);

        public List<object> EqualityValues { get; set; } = new List<object>();

        public ValueComparison ValueComparison { get; set; }

        internal string ToColumnSeparatedString()
        {
            var hash = ClearForHash(Key) + ":";

            hash += ((int)ValueComparison).ToString() + ":";
            hash += ValueType.FullName + ":";
            hash += ClearForHash(Maximum) + ":";
            hash += ClearForHash(Minimum) + ":";

            var eq = "";
            var sep = "";

            foreach (var value in EqualityValues)
            {
                eq += sep + ClearForHash(value);
                sep = ":";
            }

            hash += eq;

            return hash;
        }

        private string ClearForHash(object value)
        {
            var v = value?.ToString() ?? "";
            
            v = v.ToLower() ?? "";

            return v;
        }

        public override string ToString()
        {
            switch (ValueComparison)
            {
                case ValueComparison.BetweenValues:
                    return Minimum + " > " + Key + " > " + Maximum;
                case ValueComparison.LargerThan:
                    return Key + " > " + Minimum;
                case ValueComparison.SmallerThan:
                    return Key + " < " + Maximum;
                case ValueComparison.Equal:
                    return  Key + " = " + string.Join(" | ",EqualityValues);
                case ValueComparison.NotEqual:
                    return Key += "!=" + string.Join(" | ",EqualityValues);
            }

            return "";
        }
    }
}