using GreenPipes.Internals.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Navira.Shop.Core.Extensions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Navira.Shop.Core.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AdjustStringAttribute : Attribute
    {
        public string RegexPattern { get; set; }
        public string Replacement { get; set; }
        public bool DontZiroStringToNull { get; set; }
        public bool DontFixYeKe { get; set; }

        public AdjustStringAttribute()
        {
            RegexPattern = $@"[$&+,:;=?\[\]@#|{{}}'<>^*%!{"\u200e"}{(char)10}{(char)13}]";
            Replacement = "";
        }
        public AdjustStringAttribute(bool dontZiroStringToNull)
        {
            DontZiroStringToNull = dontZiroStringToNull;
        }
        public AdjustStringAttribute(string regex)
        {
            RegexPattern = regex;
            Replacement = "";
        }
        public AdjustStringAttribute(string regex, string replacement)
        {
            RegexPattern = regex;
            Replacement = replacement;
        }
        public AdjustStringAttribute(string regex, string replacement, bool dontZiroStringToNull)
        {
            RegexPattern = regex;
            Replacement = replacement;
            DontZiroStringToNull = dontZiroStringToNull;
        }
    }

    public static class AdjustHelper
    {
        public static void AdjustStringFeild<T>(this T model) where T : class
        {
            foreach (var field in typeof(T).GetProperties())
            {
                if (field.PropertyType != typeof(string))
                    continue;
                var value = (string)field.GetValue(model);
                if (value.IsNullOrWhiteSpace())
                    continue;
                if (field.HasAttribute<AdjustStringAttribute>())
                {
                    var attributes = field.GetCustomAttributes<AdjustStringAttribute>();
                    if (attributes.IsNotNullOrEmpty())
                        foreach (var attribute in attributes)
                        {
                            var pattern = attribute.RegexPattern;
                            var replacement = attribute.Replacement;
                            var dontZiroStringToNull = attribute.DontZiroStringToNull;
                            if (pattern.IsNotNullOrWhiteSpace())
                                value = Regex.Replace(value, $"{pattern}", replacement);
                            if (!dontZiroStringToNull && value.All(x => x == '0'))
                                value = null;
                            if (!attribute.DontFixYeKe)
                                value = value.ApplyCorrectYeKe();
                        }
                }
                else if (value.All(x => x == '0'))
                    value = null;
                else
                    value.ApplyCorrectYeKe();

                field.SetValue(model, value);
            }
        }

        public static void AdjustStringEntityPropertyValues(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.AdjustStringFeild();
                }
            }
        }

    }
}
