using COMMON;
using CONNECTION_FACTORY.DB_CONNECTION_FACTORY;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONNECTION_FACTORY.DAL_SESSION
{
    public class DalSession:IDalSession
    {
        private IDbConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public DalSession(IDbConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection Connection() 
        {
            if (_connection is null)
                _connection = _connectionFactory.CreateConnection();
            if(_connection.State == ConnectionState.Closed)
            {
                _connection.ConnectionString = GlobalApp.AppSettings.ConnectionInfo.ConnectionString;
                _connection.Open();
            }
            return _connection;
        
        }
        public IDbTransaction Tasnsaction() 
        {
            return _transaction ?? _connection.BeginTransaction();
        

        } 

        public void Begin()
        {
            if(_connection is null || _connection.State == ConnectionState.Closed || _transaction is not null)
            {
                Dispose();
                _connection = Connection();
            }
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                Dispose();
            }
        }



        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
            if (_connection == null)
                return;
            _connection.Close();
            _connection.Dispose();
        }
    }
}
