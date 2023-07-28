using COMMON.ATTRIBUTES;
using COMMON.HELPER;
using COMMON.QUERY_OBJECTS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.BUILDER
{
    public static class QueryBuilder
    {
        #region Getters

        public static QueryParams ToSelectQueryById<T>(object id,int isDeleted = 0)
        {
            var keyProp = ReflectionHelper.GetKeyProperty<T>();
            var p = new ExpandoObject();
            p.TryAdd(keyProp.Name, Convert.ChangeType(id, keyProp.PropertyType));


            return p.ToSelectQueryByObject<T>(1,1,false,isDeleted);
        }
        public static QueryParams SelectCount<T>(dynamic dynamicObjects = null)
        {
            if(dynamicObjects is null)
            {
                return new QueryParams() { Parameters = null, Query = $"Select count(*) as Res from [{typeof(T).GetTableName()}]" };
            }
            var selectObject = BuildWhereQueryByObject<T>(dynamicObjects);

            return new QueryParams() { Parameters = selectObject.Parameters, Query = $"select count(*) as Res from [{typeof(T).GetTableName()}] {selectObject.Query}" };
        }
        public static QueryParams ToSelectQueryByKeyValues<T>(this Dictionary<string, string> keyValues,int isDeleted=0) 
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(BuildSelectQuery<T>());
            var sqlWhere = keyValues.BuildWhereQueryByKeyValues<T>(isDeleted);
            queryBuilder.Append(sqlWhere.Query);
            queryBuilder.Append(keyValues.ToOrderQry<T>());
            queryBuilder.Append(keyValues.ToPagination());
            
            return new QueryParams() { Parameters = sqlWhere.Parameters , Query = queryBuilder.ToString()};
        }

        public static QueryParams ToSelectQueryByQuery<T>(this string query, dynamic props = null)
        {

            return new QueryParams() { Parameters = props, Query = query};
        }
        public static string BuildSelectQuery<T>()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("Select [COLUMNS] from [TABLE]");
            queryBuilder.Replace("[TABLE]", $"[{typeof(T).GetTableName()}]");
            queryBuilder.Replace("[COLUMNS]", String.Join(",", typeof(T).GetDatabaseColumns()));
            return queryBuilder.ToString(); 
        }
        public static QueryParams BuildWhereQueryByKeyValues<T>(this Dictionary<string,string> keyValues, int isDeleted = 0)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(" where 1=1");
            var propColumns = typeof(T).GetPropertyColumns();
            var whereProps = new ExpandoObject();
            foreach(var keyValue in keyValues)
            {
                var keys = keyValue.Key.Split(':');
                var prop = propColumns.FirstOrDefault(x => x.Name.Equals(keys[0], StringComparison.OrdinalIgnoreCase));
                if (prop is not null)
                {
                    var column = prop.GetColumnName();
                    if (keys.Length == 1)
                    {
                        var value = Convert.ChangeType(keyValue.Value, prop.PropertyType);
                        if (keyValue.Value.ToLower() == "null")
                        {
                            queryBuilder.AppendLine($"and {column} is null");
                            continue;
                        }
                        else
                        {
                            queryBuilder.AppendLine($"and {column} = @{prop.Name}");
                        }
                        whereProps.TryAdd(prop.Name, value);
                    }
                    else if (keys.Length == 2)
                    {
                        var key = keys[0];
                        var operation = keys[1].ToLower();
                        switch (operation)
                        {
                            case ">":
                            case ">=":
                            case "<":
                            case "<=":
                            case "!=":
                            case "<>":
                                queryBuilder.AppendLine($"and {column} {operation} @{prop.Name}");
                                whereProps.TryAdd(prop.Name, Convert.ChangeType(keyValue.Value, prop.PropertyType));
                                break;
                            case "in":
                            case "notin":
                                queryBuilder.AppendLine($"and {column} {operation} @{prop.Name}");

                                var queryInValues = keyValue.Value.Split(",");
                                object[] inValues = new object[queryInValues.Length];
                                for (int i = 0; i < inValues.Length; i++)
                                {
                                    inValues[i] = Convert.ChangeType(queryInValues[i], prop.PropertyType);
                                }
                                whereProps.TryAdd(prop.Name, inValues);
                                break;
                            case "between":
                            case "notbetween":
                                var betweenValues = keyValue.Value.Split(",");
                                if (betweenValues.Length != 2)
                                    throw new Exception($"operation : {operation} must be contain 2 values");

                                var firstKey = $"First{prop.Name}";
                                var lastKey = $"Last{prop.Name}";
                                whereProps.TryAdd(firstKey, Convert.ChangeType(betweenValues[0], prop.PropertyType));
                                whereProps.TryAdd(lastKey, Convert.ChangeType(betweenValues[1], prop.PropertyType));
                                if (operation == "between")
                                    queryBuilder.AppendLine($"and ({column} >= @{firstKey} and  {column} <= @{lastKey})");
                                else
                                    queryBuilder.AppendLine($"and not ({column} > @{firstKey} and  {column} < @{lastKey})");
                                break;
                            case "like":
                                whereProps.TryAdd(prop.Name, Convert.ChangeType($"%{keyValue.Value}%", prop.PropertyType));
                                queryBuilder.AppendLine($"and {column} like @{prop.Name}");
                                break;
                            case "isnotnull":
                                queryBuilder.AppendLine($"and {column} is not null");
                                break;
                        }

                    }
                    else
                    {
                        throw new Exception($"Invalid operation on {keyValue.Key} = {keyValue.Value}");

                    }
                }
                else
                    continue;
            }

            if (typeof(T).CheckIsSoft())
            {
                queryBuilder.AppendLine($"and {QueryHelper.SoftDeleteQuery<T>(isDeleted)}");
            }
            return new QueryParams() { Parameters = whereProps , Query= queryBuilder.ToString() };
        }


        public static QueryParams ToSelectQueryByObject<T>(this object dynamicObject , int page =1 , int pageSize = 10,bool getAll=false,int isDeleted=0) 
        {
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.AppendLine(BuildSelectQuery<T>());
            var whereQry = BuildWhereQueryByObject<T>(dynamicObject,isDeleted);
            queryBuilder.AppendLine(whereQry.Query);
            if (!getAll)
            {
                queryBuilder.AppendLine("Order by (Select null)");
                queryBuilder.AppendLine(ToPagination(page, pageSize));

            }
            return new QueryParams() { Query = queryBuilder.ToString() , Parameters = dynamicObject};
        }

        public static QueryParams BuildWhereQueryByObject<T>(dynamic dynamicObject, int isDeleted = 0)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("where 1=1");
            ExpandoObject p = new ExpandoObject();
            string jsonString = JsonConvert.SerializeObject(dynamicObject);

            var dynamicInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            foreach (var prop in dynamicInfo)
            {
                var propEntity = prop.Key.GetProperty<T>();
                if (propEntity is not null)
                {
                    if (prop.Value != null)
                    {
                        queryBuilder.AppendLine($"and {propEntity.GetColumnName()} = @{propEntity.Name}");
                        p.TryAdd(propEntity.Name, Convert.ChangeType(prop.Value, propEntity.PropertyType));
                    }
                    else
                    {
                        queryBuilder.AppendLine($"and {propEntity.GetColumnName()} is null");
                    }
                }
            }
            if (typeof(T).CheckIsSoft())
            {
                queryBuilder.AppendLine($"and {QueryHelper.SoftDeleteQuery<T>(isDeleted)}");
            }

            return new QueryParams() { Parameters = p, Query = queryBuilder.ToString() };
        }
        public static QueryParams BuildWhereQueryByString<T>(this string whereQry,dynamic props = null, int page = 1, int pageSize = 10, bool getAll = false , int isDeleted=0)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(whereQry);
            if (typeof(T).CheckIsSoft())
            {
                queryBuilder.AppendLine($"and {QueryHelper.SoftDeleteQuery<T>(isDeleted)}");
            }
            if (!getAll)
            {
                queryBuilder.AppendLine("Order by (Select null)");
                queryBuilder.AppendLine(ToPagination(page, pageSize));
            }

            return new QueryParams()
            { 
                Parameters = props, 
                Query = queryBuilder.ToString() 
            };
        }

        public static string ToOrderQry<T>(this Dictionary<string, string> keyValues) 
        {
            var orderByKeyValue = keyValues.FirstOrDefault(x => x.Key.Equals("orderby", StringComparison.OrdinalIgnoreCase));
            var orderDirKeyValue = keyValues.FirstOrDefault(x => x.Key.Equals("orderdir", StringComparison.OrdinalIgnoreCase));
            if (orderByKeyValue.Key != null)
            {
                StringBuilder orderBuilder = new StringBuilder();
                var orderByProp = orderByKeyValue.Value.GetProperty<T>();
                if(orderByProp is null)
                {
                    throw new Exception($"{orderByKeyValue.Value} is not column");
                }
                var orderByColumn = orderByProp.GetColumnName();
                string orderDir = "asc";
                if(orderDirKeyValue.Value is not null && orderDirKeyValue.Value.ToLower() == "desc")
                    orderDir = "desc";

                orderBuilder.AppendLine($"order by {orderByColumn} {orderDir}");

                return orderBuilder.ToString();

            }
            return $"order by (Select null)";
        }
        public static string ToPagination(int page , int pageSize)
        {
            page = page == 0 ? 1 : page;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int offset = (page - 1) * pageSize;
            return $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        }
        public static string ToPagination(this Dictionary<string, string> keyValues)
        {
            var pageKeyValue = keyValues.FirstOrDefault(x=>x.Key.Equals("page",StringComparison.OrdinalIgnoreCase));
            var pageSizeKeyValue = keyValues.FirstOrDefault(x=>x.Key.Equals("pagesize",StringComparison.OrdinalIgnoreCase));
            var getAllKeyValue = keyValues.FirstOrDefault(x=>x.Key.Equals("getall",StringComparison.OrdinalIgnoreCase));
            if(getAllKeyValue.Key == null)
            {
                int.TryParse(pageKeyValue.Value, out int page);
                int.TryParse(pageSizeKeyValue.Value, out int pageSize);
                page= page== 0 ? 1 : page;
                pageSize = pageSize == 0 ? 10 : pageSize;
                int offset = (page - 1) * pageSize;
                return $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }
            return string.Empty;
        }
        #endregion


        public static QueryParams InsertQuery<T>(this T entity)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"Insert into [{typeof(T).GetTableName()}]  ");
            ExpandoObject p = new ExpandoObject();
            var props = typeof(T).GetPropertyColumns();
            var columnToInsert = new List<string>();
            var propsToInsert = new List<string>();
            foreach (var prop in props)
            {
                if (prop.HasAttribute<DatabaseGeneratedAttribute>())
                {
                    var databaseGen = prop.GetAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGen.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        continue;
                }
                var value = prop.GetValue(entity);
                p.TryAdd(prop.Name,value);
                columnToInsert.Add(prop.GetColumnName());
                propsToInsert.Add($"@{prop.Name}");

            }

            queryBuilder.Append($"({String.Join(',',columnToInsert)})");
            queryBuilder.Append($" Values ({String.Join(',',propsToInsert)});");
            queryBuilder.Append($"Select SCOPE_IDENTITY() as Res");
            return new QueryParams() { Parameters = p , Query = queryBuilder.ToString() };
        }

        public static QueryParams UpdateQuery<T>(this T entity)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine($"Update [{typeof(T).GetTableName()}] Set");
            var props = typeof(T).GetPropertyColumns();
            var columnToUpdate = new List<string>();
            var keyProp = ReflectionHelper.GetKeyProperty<T>();
            object id = 0;
            foreach(var prop in props)
            {
                if (prop.Name == keyProp.Name)
                {
                    id = prop.GetValue(entity);
                    continue;

                }
                columnToUpdate.Add($"{prop.GetColumnName()} = @{prop.Name}");
            }
            queryBuilder.AppendLine(String.Join(',', columnToUpdate));
            queryBuilder.AppendLine($"where {keyProp.GetColumnName()} = @{keyProp.Name};");

            var selectById = ToSelectQueryById<T>(id);
            queryBuilder.AppendLine(selectById.Query);
            return new QueryParams() { Parameters = entity , Query= queryBuilder.ToString() };
            
        }

        public static QueryParams DeleteByQuery<T>(this Dictionary<string , string> keyValuePairs)
        {

            var queryBuilder = new StringBuilder();
            var propKey = ReflectionHelper.GetKeyProperty<T>();
            

            var whereQry = keyValuePairs.BuildWhereQueryByKeyValues<T>();

            if (typeof(T).HasAttribute<SoftDeleteAttribute>())
            {
                queryBuilder.AppendLine($"Update [{typeof(T).GetTableName()}]");
                queryBuilder.AppendLine($"Set {QueryHelper.SoftDeleteQuery<T>(1)}");

            }
            else
            {
                queryBuilder.AppendLine($"Delete from [{typeof(T).GetTableName()}]");
            }
            queryBuilder.AppendLine($"{whereQry.Query};") ;
            return new QueryParams() { Parameters = whereQry.Parameters, Query = queryBuilder.ToString()};


        }

        public static QueryParams DeleteByIdQuery<T>(object id)
        {

            var queryBuilder = new StringBuilder();
            var propKey = ReflectionHelper.GetKeyProperty<T>();
            if (typeof(T).HasAttribute<SoftDeleteAttribute>())
            {
                queryBuilder.AppendLine($"Update [{typeof(T).GetTableName()}]");
                queryBuilder.AppendLine($"Set {QueryHelper.SoftDeleteQuery<T>(1)}");

            }
            else
            {

                queryBuilder.AppendLine($"Delete from [{typeof(T).GetTableName()}]");
            }
            queryBuilder.AppendLine($"where {propKey.GetColumnName()} = @{propKey.Name};");

            ExpandoObject keyValue = new ExpandoObject();
            keyValue.TryAdd(propKey.Name, Convert.ChangeType(id, propKey.PropertyType));

            return new QueryParams() { Parameters = keyValue, Query = queryBuilder.ToString() };


        }


    }
}
