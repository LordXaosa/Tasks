using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public static class JsonConfigurate
    {
        public static JsonSerializerSettings JsonSettings { get; private set; } = new JsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = ConfigHelper.Instance.IncludeNullInJson ? NullValueHandling.Include : NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new ExceptionResolver(),
            TypeNameHandling = TypeNameHandling.All
        };
    }
    public class ExceptionResolver : DefaultContractResolver
    {
        public ExceptionResolver() : base()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
            IgnoreSerializableInterface = true;
        }
        protected override JsonProperty CreateProperty(MemberInfo member,
                                            MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Exception) && property.PropertyName == "targetSite")
            {
                property.ShouldSerialize = instanceOfProblematic => false;
            }

            return property;
        }
    }
}
