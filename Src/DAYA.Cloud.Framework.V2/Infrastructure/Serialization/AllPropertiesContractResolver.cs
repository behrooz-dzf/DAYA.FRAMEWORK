﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAYA.Cloud.Framework.V2.Infrastructure.Serialization;

public class AllPropertiesContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var properties = type.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
            .Select(p => base.CreateProperty(p, memberSerialization))
            .ToList();

        properties.ForEach(p =>
        {
            p.Writable = true;
            p.Readable = true;
        });

        return properties;
    }
}
