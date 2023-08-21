using System;

namespace Acidmanic.Utilities.Filtering.Attributes
{
    public class FilterFieldAttribute : Attribute
    {
        public FilterFieldAttribute(ValueComparison method)
        {
            Method = method;
        }

        public FilterFieldAttribute() : this(ValueComparison.Equal)
        {
        }

        public ValueComparison Method { get; private set; }
    }
}