using Dapper;
using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(
            this IEnumerable<T> enumerable,
            string typeName,
            IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();

            if (typeof(T).IsValueType || typeof(T).FullName.Equals("System.String"))
            {
                dataTable.Columns.Add(orderedColumnNames == null ? "NONAME" : orderedColumnNames.First(), typeof(T));
                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(obj as object);
                }
            }
            else
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var readableProperties = properties.Where(w => w.CanRead).ToArray();

                if (readableProperties.Length > 1 && orderedColumnNames == null)
                    throw new ArgumentException(@"Ordered list of column names must be provided when TVP contains more than one column");

                var columnNames = (orderedColumnNames ?? readableProperties.Select(s => s.Name)).ToArray();

                foreach (string name in columnNames)
                {
                    dataTable.Columns.Add(name, readableProperties.Single(s => s.Name.Equals(name)).PropertyType);
                }

                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(columnNames
                        .Select(s => readableProperties.Single(s2 => s2.Name.Equals(s)).GetValue(obj)).ToArray());
                }
            }

            return dataTable.AsTableValuedParameter(typeName);
        }

        public static async Task UseStoredProcedureProcess<T>(
            this T result,
            string connectionString,
            Func<SqlConnection, DynamicParameters, T, Task> func,
            string resultCodePropertyName = "@RESULTCODE",
            string resultMessagePropertyName = "@RESULTMESSAGE",
            string resultDetailPropertyName = "@RESULTDETAIL")
            where T : IResult
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add(resultCodePropertyName, null, DbType.Int32, ParameterDirection.Output);
                parameters.Add(resultMessagePropertyName, null, DbType.String, ParameterDirection.Output, 500);
                parameters.Add(resultDetailPropertyName, null, DbType.String, ParameterDirection.Output, 4000);

                await func(connection, parameters, result);

                result.Code = (int)(Enum.TryParse<ResultType>(
                    parameters.Get<int>(resultCodePropertyName).ToString(), out var resultCode) ?
                    resultCode :
                    ResultType.DatabaseError);
                result.Message = parameters.Get<string>(resultMessagePropertyName);
                result.DetailMessage = parameters.Get<string>(resultDetailPropertyName);
            }
        }
    }
}
