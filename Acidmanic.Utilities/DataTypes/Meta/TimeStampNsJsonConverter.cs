using System;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.DataTypes.Meta
{
    internal class TimeStampNsJsonConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TimeStamp timeStamp)
            {
                var stringValue = timeStamp.TotalMilliSeconds.ToString();
                
                writer.WriteValue(stringValue);
                
                return;
            }
            writer.WriteValue("0");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var strValue = (string)reader.Value;

            if (long.TryParse(strValue, out var milliseconds))
            {

                return (TimeStamp)milliseconds;
            }

            return (TimeStamp)0L;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeStamp);
        }
    }
    
    
}