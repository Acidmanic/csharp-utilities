using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Acidmanic.Utilities.DataTypes.Meta
{
    internal class TimeStampMsJsonConverter:JsonConverter<TimeStamp>
    {
        public override TimeStamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt64(out var milliseconds))
            {
                return milliseconds;
            }

            return 0;
        }

        public override void Write(Utf8JsonWriter writer, TimeStamp value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.TotalMilliSeconds);
        }
    }
}