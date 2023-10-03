using System;
using System.Collections.Generic;
using Acidmanic.Utilities.Reflection.Attributes;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.DataTypes
{
    [AlteredType(typeof(string))]
    public class JsonData
    {
        public static readonly Dictionary<string, Type> TypesByName = new Dictionary<string, Type>();

        public string Raw { get; set; }


        public JsonData()
        {
            Raw = Serialize(new object());
        }
        
        private class JsonDataHeader
        {
            public Type Type { get; set; }
        }

        private class JsonDataWrap<T> : JsonDataHeader
        {
            public T Model { get; set; }
        }

        private static Type SpecificWrapType(Type modelType)
        {
            var genericType = typeof(JsonDataWrap<>);

            var specificType = genericType.MakeGenericType(modelType);

            return specificType;
        }

        private static object CreateJsonWrap(Type type)
        {
            var specificType = SpecificWrapType(type);

            var wrap = specificType.GetConstructors()[0].Invoke(new object[] { });

            return wrap;
        }

        private static object Wrap(object model, Type type)
        {
            var wrap = CreateJsonWrap(type);

            var wrapType = wrap.GetType();

            var modelProperty = wrapType.GetProperty(nameof(JsonDataWrap<object>.Model));

            var typeProperty = wrapType.GetProperty(nameof(JsonDataWrap<object>.Type));

            modelProperty?.SetValue(wrap, model);

            typeProperty?.SetValue(wrap, type);

            return wrap;
        }


        protected static string Serialize(object model)
        {
            if (model == null)
            {
                return Serialize(new object());
            }

            var wrap = Wrap(model, model.GetType());

            var json = JsonConvert.SerializeObject(wrap);

            var compressed = json.CompressAsync(Compressions.GZip).Result;

            return compressed;
        }

        protected static object Deserialize(string compressedJson)
        {
            var json = compressedJson.DecompressAsync(Compressions.GZip).Result;

            var wrap = JsonConvert.DeserializeObject<JsonDataHeader>(json);

            var modelType = wrap?.Type ?? typeof(object);

            var wrapType = SpecificWrapType(modelType);

            var specificWrap = JsonConvert.DeserializeObject(json, wrapType);

            var modelProperty = wrapType.GetProperty(nameof(JsonDataWrap<object>.Model));

            return modelProperty?.GetValue(specificWrap);
        }


        public static implicit operator string(JsonData data)
        {
            return data.Raw;
        }

        public static implicit operator JsonData(string raw)
        {
            return new JsonData
            {
                Raw = raw
            };
        }
    }

    public class JsonData<TModel> : JsonData where TModel : class
    {
        public static implicit operator TModel(JsonData<TModel> data)
        {
            return data.Value;
        }

        public static implicit operator JsonData<TModel>(TModel model)
        {
            if (model == default)
            {
                return null;
            }

            var jsonData = new JsonData<TModel>();

            jsonData.Value = model;

            return jsonData;
        }


        public TModel Value
        {
            get
            {
                var o = Deserialize(this.Raw);

                if (o == null)
                {
                    return default;
                }

                return o as TModel;
            }
            set { Raw = Serialize(value); }
        }
    }
}