using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONNECTION_FACTORY.DB_CONNECTION_FACTORY
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
