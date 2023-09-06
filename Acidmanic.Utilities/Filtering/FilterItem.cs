using System;
using System.Collections.Generic;

namespace Acidmanic.Utilities.Filtering
{
    public class FilterItem
    {
        private static readonly string[] OutOfHashStrings = { ":" };
        public string Key { get; set; }

        public string Maximum { get; set; }

        public string Minimum { get; set; }

        public Type ValueType { get; set; } = typeof(string);

        public List<string> EqualValues { get; set; } = new List<string>();

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

            foreach (var value in EqualValues)
            {
                eq += sep + ClearForHash(value);
                sep = ":";
            }

            hash += eq;

            return hash;
        }

        private string ClearForHash(string value)
        {
            value = value?.ToLower() ?? "";

            return value;
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
                    return  Key + " = " + string.Join(" | ",EqualValues);
            }

            return "";
        }
    }
}