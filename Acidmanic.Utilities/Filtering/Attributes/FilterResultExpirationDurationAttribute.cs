using System;

namespace Acidmanic.Utilities.Filtering.Attributes
{
    public class FilterResultExpirationDurationAttribute:Attribute
    {
        public FilterResultExpirationDurationAttribute(long milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public long Milliseconds { get; }
    }
}