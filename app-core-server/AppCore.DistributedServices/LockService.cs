using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public class LockService : IDisposable, ILockService
    {
        private string _connectionString;
        private int _timeOut = 0;
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;

        public LockService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AquireLock(string uid)
        {
            if (_dbConnection == null)
            {
                _dbConnection = new SqlConnection(_connectionString);
                _dbConnection.Open();
                _transaction = _dbConnection.BeginTransaction();
            }
            IDbCommand cmd = _dbConnection.CreateCommand();
            cmd.Transaction = _transaction;
            cmd.CommandText = "DECLARE @result int; ";
            cmd.CommandText += "EXEC @result = sp_getapplock @Resource = @LockName, @LockMode = 'Exclusive', @LockTimeout = @TimeOut; ";
            cmd.CommandText += "select @result";
            cmd.Parameters.Add(new SqlParameter("@LockName", uid));
            cmd.Parameters.Add(new SqlParameter("@TimeOut", _timeOut));

            object result = cmd.ExecuteScalar();

            return (Convert.ToInt32(result) >= 0);
        }

        public void ReleaseLock(string uid)
        {
            if (_dbConnection == null)
            {
                IDbCommand cmd = _dbConnection.CreateCommand();
                cmd.Transaction = _transaction;
                cmd.CommandText = "EXEC sp_releaseapplock @Resource = @LockName";
                cmd.Parameters.Add(new SqlParameter("@LockName", uid));
                cmd.ExecuteNonQuery();
            }
        }


        public void Dispose()
        {
            ReleaseAllLocks();
        }

        public void ReleaseAllLocks()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                    _transaction = null;
                }
                catch { }
            }

            if (_dbConnection != null)
            {
                try
                {
                    _dbConnection.Close();
                    _dbConnection = null;
                }
                catch { }
            }
        }
    }
}
