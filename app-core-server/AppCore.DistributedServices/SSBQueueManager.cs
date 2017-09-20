using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public class SSBQueueManager : IQueueManager
    {

        protected readonly string _connectionString;

        protected readonly string _receiveService;

        protected readonly string _queueName;

        protected List<ItemInfo> _inProcItems = new List<ItemInfo>();

        public SSBQueueManager(string connectionString, string queueName, string receiveService)
        {
            _connectionString = connectionString;
            _receiveService = receiveService;
            _queueName = queueName;
        }                
                

        public void Enqueue(int obj)
        {
            lock (this)
            {
                SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "[dbo].[EnqueueMessage] @Service = @Service, @ID = @ID";
                    cmd.Parameters.Add(new SqlParameter("@Service", _receiveService));
                    cmd.Parameters.Add(new SqlParameter("@ID", obj));
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public ReceivedMessage TryDequeue(int timeout)
        {            
            SqlConnection conn = new SqlConnection(_connectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();


            SqlTransaction txn = conn.BeginTransaction();
            //cmd.CommandText = "WAITFOR (RECEIVE TOP(1) CONVERT(INT, message_body) FROM [dbo]." + QueueName + " ),TIMEOUT " + timeout.ToString() + " ;";            
            cmd.CommandText = "RECEIVE TOP(1) CONVERT(INT, message_body) FROM [dbo]." + _queueName + ";"; // " WHERE message_enqueue_time < (DATEADD(SECOND, -5, getdate()));";

            cmd.Transaction = txn;
            object result = cmd.ExecuteScalar();

            if (result is int)
            {
                var item = new ReceivedMessage(Convert.ToInt32(result));
                _inProcItems.Add(new ItemInfo() { Message = item, Transaction = txn, Connection = conn });
                return item;
            }

            txn.Rollback();
            conn.Close();
            return null;
        }
                

        
        public void Ack(ReceivedMessage item)
        {
            var message = _inProcItems.FirstOrDefault(x => x.Message == item);
            if (message != null)
            {                
                if (message.Transaction != null)
                {
                    message.Transaction.Commit();
                    CloseConnection(message);
                }
                _inProcItems.Remove(message);
            }
        }

        public void NAck(ReceivedMessage item)
        {
            var message = _inProcItems.FirstOrDefault(x => x.Message == item);
            if (message != null)
            {
                if (message.Transaction != null)
                {
                    message.Transaction.Rollback();
                    CloseConnection(message);
                }
                _inProcItems.Remove(message);
            }
        }

        public void Requeue(ReceivedMessage item)
        {
            var message = _inProcItems.FirstOrDefault(x => x.Message == item);
            if (message != null)
            {
                Ack(item);
                System.Threading.Thread.Sleep(2000);
                Enqueue(item.Payload);
            }
        }

        //public int Count()
        //{
        //    DataService dataService = _dataServiceFactory.CreateDataService();
        //    int result = dataService.Context.ExecuteStoreQuery<int>("SELECT count(*) FROM [dbo]." + QueueName + " WITH(NOLOCK)").First();
        //    return result;
        //}

        private void CloseConnection(ItemInfo item)
        {
            try
            {
                item.Connection.Close();
            }
            catch
            {
            }
        }


        public class ItemInfo
        {
            public ReceivedMessage Message { get; set; }            
            public DbTransaction Transaction { get; set; }
            public DbConnection Connection { get; set; }
        }

    }
}
