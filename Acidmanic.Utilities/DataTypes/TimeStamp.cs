using System;
using Acidmanic.Utilities.DataTypes.Meta;
using Acidmanic.Utilities.Reflection.Attributes;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.DataTypes
{
    
    public static class DateTimeExtensions
    {
        public static long GetTotalMilliseconds(this DateTime dateTime)
        {
            var baseDate = new DateTime(1970, 1, 1);

            var difference = dateTime.Subtract(baseDate).TotalMilliseconds;

            return (long) difference;
        }
    }
    
    /// <summary>
    /// This class stores a point in time, be represented as a <code>long</code> value
    ///  or a <code>string</code> value or <code>DateTime</code> or <code>TimeSpan</code>.
    ///  It's directly cast-able to/from <code>long</code> and <code>string</code> and
    /// <code>DateTime</code> and <code>TimeSpan</code>.
    /// Converted to long, will give the total milliseconds. 
    /// </summary>
    [JsonConverter(typeof(TimeStampNsJsonConverter))]
    [AlteredType(typeof(long))]
    [System.Text.Json.Serialization.JsonConverter(typeof(TimeStampMsJsonConverter))]
    public class TimeStamp
    {
        public long TotalMilliSeconds { get; }

        public TimeStamp(long totalMilliSeconds)
        {
            TotalMilliSeconds = totalMilliSeconds;
        }


        public static implicit operator TimeStamp(long timestamp)
        {
            return new TimeStamp(timestamp);
        }

        public static implicit operator long(TimeStamp timeStamp)
        {
            return timeStamp.TotalMilliSeconds;
        }

        public static implicit operator TimeStamp(string timestamp)
        {
            var tsLong = long.Parse(timestamp);

            return new TimeStamp(tsLong);
        }

        public static implicit operator string(TimeStamp timeStamp)
        {
            return timeStamp.TotalMilliSeconds.ToString();
        }

        public static implicit operator TimeStamp(DateTime timestamp)
        {
            return timestamp.GetTotalMilliseconds();
        }

        public static implicit operator DateTime(TimeStamp timeStamp)
        {
            var ticks = TimeSpan.TicksPerMillisecond * timeStamp.TotalMilliSeconds;

            var ts = new TimeSpan(ticks);

            var date = DateTime.UnixEpoch.Add(ts);

            return date;
        }

        public static implicit operator TimeStamp(TimeSpan timestamp)
        {
            return (long)timestamp.TotalMilliseconds;
        }

        public static implicit operator TimeSpan(TimeStamp timeStamp)
        {
            var ticks = TimeSpan.TicksPerMillisecond * timeStamp.TotalMilliSeconds;

            return new TimeSpan(ticks);
        }

        public static TimeStamp Now => DateTime.Now;


        public long CompareTo(TimeStamp value)
        {
            return TotalMilliSeconds - value.TotalMilliSeconds;
        }


        public long CompareTo(TimeSpan value)
        {
            return TotalMilliSeconds - (long)value.TotalMilliseconds;
        }

        public long CompareTo(DateTime value)
        {
            return TotalMilliSeconds - value.GetTotalMilliseconds();
        }
    }
}