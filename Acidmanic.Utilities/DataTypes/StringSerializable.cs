using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Acidmanic.Utilities.Extensions;
using Acidmanic.Utilities.Reflection.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Acidmanic.Utilities.DataTypes;

[AlteredType(typeof(string))]
public class StringSerializableData
{
   public string StringValue { get;  set; }
   
   public static implicit operator string(StringSerializableData serializable)
   {
       return serializable.StringValue;
   }

   public static implicit operator StringSerializableData(string value)
   {
       var serializable = new StringSerializableData
       {
           StringValue = value
       };

       return serializable;
   }
}

[AlteredType(typeof(string))]
public abstract class StringSerializable:StringSerializableData
{
    protected static readonly string EmptyObjectRaw = Serialize(new object());
    
    private static string Serialize(object model)
    {
        if (model == null)
        {
            return EmptyObjectRaw;
        }

        byte[] bytes;

        using (MemoryStream ms = new MemoryStream())
        {
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.Serialize(writer, model);
            }

            bytes = ms.ToArray();
        }

        bytes = bytes.Compress();

        var base64 = Convert.ToBase64String(bytes);

        return base64;
    }

    private static object Deserialize(string serialized, [AllowNull] Type? modelType = null)
    {
        if (!string.IsNullOrEmpty(serialized))
        {
            var bytes = Convert.FromBase64String(serialized);

            bytes = bytes.Decompress();

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (BsonDataReader reader = new BsonDataReader(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    object model = modelType == null
                        ? serializer.Deserialize(reader)
                        : serializer.Deserialize(reader, modelType);

                    if (model != null)
                    {
                        return model;
                    }
                }
            }
        }
        return new object();
    }

     

    protected abstract Type? GetModelType();

    
    
    [JsonIgnore] 
    [System.Text.Json.Serialization.JsonIgnore]
    public object? ModelObject
    {
        get => Deserialize(StringValue, GetModelType());
        set => StringValue = Serialize(value);
    }
}



[AlteredType(typeof(string))]
public class StringSerializable<TModel> : StringSerializable where TModel : class
{
    
    
    [JsonIgnore] 
    [System.Text.Json.Serialization.JsonIgnore] 
    public TModel? Model { 
        get => ModelObject as TModel;
        set => ModelObject = value;
    }
    
    
    protected override Type GetModelType()
    {
        return typeof(TModel);
    }

    public static implicit operator string(StringSerializable<TModel> serializable)
    {
        return serializable.StringValue;
    }

    public static implicit operator StringSerializable<TModel>(string value)
    {
        var serializable = new StringSerializable<TModel>();

        serializable.StringValue = value;

        return serializable;
    }

    public static implicit operator TModel?(StringSerializable<TModel> serializable)
    {
        return serializable.Model;
    }

    public static implicit operator StringSerializable<TModel>(TModel model)
    {
        var serializable = new StringSerializable<TModel>();

        serializable.Model = model;

        return serializable;
    }

    public static StringSerializable<TModel> From(StringSerializableData data)
    {
        return new StringSerializable<TModel>
        {
            StringValue = data.StringValue
        };
    }
}


[AlteredType(typeof(string))]
public class StringSerializableObject : StringSerializableData
{
    
}