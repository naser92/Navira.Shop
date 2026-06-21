using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Navira.Shop.Core
{
    public class IgnoreMethodBaseResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            NamingStrategy = new CamelCaseNamingStrategy();
            // Check if the member's type inherits from MethodBase
            if (property.PropertyType != null && typeof(MethodBase).IsAssignableFrom(property.PropertyType))
            {
                // If it does, ignore the property during serialization
                property.Ignored = true;
            }

            return property;
        }
    }
}
