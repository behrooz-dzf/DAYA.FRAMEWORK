﻿using System;
using Newtonsoft.Json;

namespace DAYA.Cloud.Framework.V2.Domain;

internal class TypedIdJsonConverter : JsonConverter
{
    public TypedIdJsonConverter()
    {
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var t = value.GetType();
        var val = t.GetProperty("Value").GetValue(value, null);
        writer.WriteValue(val);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string s = reader?.Value?.ToString() ?? "";
        if (string.IsNullOrWhiteSpace(s))
        {
            return default;
        }

        return TypedIdCreator.Create(s, objectType);
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override bool CanRead
    {
        get { return true; }
    }
}