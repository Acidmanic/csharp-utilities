using System;
using System.Linq;
using System.Reflection;
using Acidmanic.Utilities.Filtering.Attributes;

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

        
        public static TimeSpan GetFilterResultExpirationTimeSpan(this Type type)
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

            return new TimeSpan(days, hours, minutes, seconds, (int) totalMilliseconds);
        }
    }
}