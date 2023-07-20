using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONNECTION_FACTORY.DAL_SESSION
{
    public interface IDalSession
    {
        IDbConnection Connection();
        IDbTransaction Tasnsaction();
        void Begin();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
