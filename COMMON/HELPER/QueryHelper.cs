using COMMON.ATTRIBUTES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.HELPER
{
    public static class QueryHelper
    {
        public static string GetTableName(this Type type) 
        {
            if(type.HasAttribute<TableAttribute>())
                return type.GetAttribute<TableAttribute>().Name;
            else return type.Name.ToUnderScores();
        }
        public static string[] GetDatabaseColumns(this Type type)
        {
            var props = type.GetProperties().Where(x => !x.HasAttribute<NotMappedAttribute>());
            var columns = new string[props.Count()];
            int i = 0;
            foreach (var prop in props)
            {
                if (prop.HasAttribute<ColumnAttribute>())
                {
                    var columnInfo = prop.GetAttribute<ColumnAttribute>();
                    columns[i] = $"{columnInfo.Name} as {prop.Name}";
                }
                else
                {
                    columns[i] = $"{prop.Name.ToUnderScores()} as {prop.Name}";
                }
                i++;
            }
            return columns;
        }

        public static string GetColumnName(this PropertyInfo prop)
        {
            if (!prop.HasAttribute<NotMappedAttribute>())
            {
                if(prop.HasAttribute<ColumnAttribute>())
                    return prop.GetAttribute<ColumnAttribute>().Name;
                return prop.Name.ToUnderScores();
            }
            throw new Exception($"{prop.Name} is not column");

        }

        public static PropertyInfo[] GetPropertyColumns(this Type type)
        {
            return type.GetProperties().Where(x => !x.HasAttribute<NotMappedAttribute>()).ToArray();
            
        }

        public static bool CheckIsSoft(this Type type)
        {
            return type.HasAttribute<SoftDeleteAttribute>();
        }

        public static string SoftDeleteQuery<T>(int isDeleted = 0)
        {
            var isDeleteProp = "isdeleted".GetProperty<T>();
            return $"{isDeleteProp.GetColumnName()} = {isDeleted}"; 

        }

    }
}
