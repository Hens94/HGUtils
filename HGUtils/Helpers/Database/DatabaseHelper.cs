using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Helpers.Database
{
    public static class DatabaseHelper
    {
        public static async Task UseTransactionAsync(this SqlConnection dbConn, Func<SqlConnection, SqlTransaction, Task> transactionAction)
        {
            dbConn.Open();
            using (var transaction = dbConn.BeginTransaction())
            {
                try
                {
                    await transactionAction(dbConn, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
