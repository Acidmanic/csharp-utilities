using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Acidmanic.Utilities.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Acidmanic.Utilities.DataTypes;

public abstract class StringSerializable
{

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

            using (MemoryStream ms = new MemoryStream())
            {
                using (BsonDataReader reader = new BsonDataReader(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    object model = modelType == null
                        ? serializer.Deserialize(reader)
                        : serializer.Deserialize(reader, modelType);

                    if (model != null)
                    {
                        return null;
                    }
                }

                bytes = ms.ToArray();
            }
        }
        return new object();
    }

    private static readonly string EmptyObjectRaw = Serialize(new object()); 

    protected abstract Type? GetModelType();

    public string StringValue { get; protected set; } = EmptyObjectRaw;
    
    public object? ModelObject
    {
        get => Deserialize(StringValue, GetModelType());
        set => StringValue = Serialize(value);
    }
}




public class StringSerializable<TModel> : StringSerializable where TModel : class
{
    
    
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
    
    
}