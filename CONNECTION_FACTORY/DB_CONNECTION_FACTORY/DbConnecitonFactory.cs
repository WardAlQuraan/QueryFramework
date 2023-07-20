using COMMON;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONNECTION_FACTORY.DB_CONNECTION_FACTORY
{
    public class DbConnectionFactory:IDbConnectionFactory
    {
        public  IDbConnection CreateConnection()
        {
            DbConnection connection = new SqlConnection(GlobalApp.AppSettings.ConnectionInfo.ConnectionString);
            return connection;
        }


    }
}
