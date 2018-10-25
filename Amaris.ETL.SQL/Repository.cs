using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Amaris.ETL.SQL
{
    public class Repository : IDisposable
    {
        private SqlConnection _db;

        public Repository(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public IEnumerable<T> Query<T>(string storedProcedureName, int offset, int batchSize)
        {
            return _db.Query<T>(storedProcedureName,
                new {Offset = offset, BatchSize = batchSize },
                commandType: CommandType.StoredProcedure).ToList();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}