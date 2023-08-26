using System;
using System.Text.Json;
using Acidmanic.Utilities.Results;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Acidmanic.Utilities.DataTypes.Meta
{
    internal class TimeStampMsJsonConverter:JsonConverter<TimeStamp>
    {
       
        public override void WriteJson(JsonWriter writer, TimeStamp value, JsonSerializer serializer)
        {
            writer.WriteValue(value.TotalMilliSeconds);
        }

        public override TimeStamp ReadJson(JsonReader reader, Type objectType, TimeStamp existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var read = ReadLong(reader);

            if (read)
            {
                return read.Value;
            }

            return 0;
        }

        private Result<long> ReadLong(JsonReader reader)
        {
            var stringValue = reader.ReadAsString();

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                if (long.TryParse(stringValue, out var milliseconds))
                {
                    return new Result<long>(true, milliseconds);
                }
            }

            return new Result<long>().FailAndDefaultValue();
        }
    }
}