using ENTITIES;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Dynamic;

#region GetByKeyValues
//var keyValues = new Dictionary<string, string>();
//keyValues.Add("pagesize", "10");
//keyValues.Add("page", "1");
//keyValues.Add("firstname:like", "ward");

//using (var con = new SqlConnection("Server=localhost;Database=Test;Trusted_Connection=True;"))
//{
//    if (con.State != System.Data.ConnectionState.Open) con.Open();
//    var selectObject = keyValues.ToSelectQuery<TestEntity>();

//    var res = await con.QueryAsync<TestEntity>(selectObject.Query, (ExpandoObject)selectObject.Parameters);
//    if (res is not null)
//    {
//        foreach (var item in res)
//        {
//            Console.WriteLine(JsonConvert.SerializeObject(item));
//        }

//    }
//}
#endregion



#region GetByDynamicObject
//var p = new { firstName = "ahmad" };
//using (var con = new SqlConnection("Server=localhost;Database=Test;Trusted_Connection=True;"))
//{
//    if (con.State != System.Data.ConnectionState.Open) con.Open();
//    var whereObject = p.BuildWhereQuery<TestEntity>();

//    var res = await con.QueryAsync<TestEntity>(whereObject.Query, whereObject.Parameters);
//    if (res is not null)
//    {
//        foreach (var item in res)
//        {
//            Console.WriteLine(JsonConvert.SerializeObject(item));
//        }

//    }
//}
#endregion

//using (var con = new SqlConnection("Server=localhost;Database=Test;Trusted_Connection=True;"))
//{
//    if (con.State != System.Data.ConnectionState.Open) con.Open();
//    var whereQry = $"where {"firstName".GetProperty<TestEntity>().GetColumnName()} = @FirstName";
//    var whereObject = whereQry.BuildWhereQueryByString<TestEntity>(new { FirstName = "ward"});

//    var res = await con.QueryAsync<TestEntity>(whereObject.WhereQuery, whereObject.Parameters);
//    if (res is not null)
//    {
//        foreach (var item in res)
//        {
//            Console.WriteLine(JsonConvert.SerializeObject(item));
//        }

//    }
//}
