using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.HELPER
{
    public static class ReflectionHelper
    {
        public static PropertyInfo? GetProperty<T>(this string propertyName) 
        {
            return typeof(T).GetProperties().FirstOrDefault(x=>x.Name.Equals(propertyName,StringComparison.OrdinalIgnoreCase));
        }

        public static PropertyInfo GetKeyProperty<T>()
        {
            var keyProp = typeof(T).GetPropertyColumns().FirstOrDefault(x => x.HasAttribute<KeyAttribute>());
            if (keyProp is null)
                throw new Exception("there is no key");
            return keyProp;

        }


        public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute 
        {
            if(property == null)
                return false;
            return property.GetCustomAttribute<T>() != null;
           
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttribute<T>() != null;

        }

        public static T? GetAttribute<T> (this PropertyInfo property) where T : Attribute
        {
            if(property.HasAttribute<T>())
                return property.GetCustomAttribute<T>() as T;
            return null;
        }

        public static T? GetAttribute<T>(this Type type) where T : Attribute
        {
            if (type.HasAttribute<T>())
                return type.GetCustomAttribute<T>() as T;
            return null;
        }
    }
}
