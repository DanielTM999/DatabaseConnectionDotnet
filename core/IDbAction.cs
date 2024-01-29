using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.core
{
    public interface IDbAction : IConnection
    {
        List<T> Execute<T>(string sql, Dictionary<string, object>? bindParams, DbConnection connection) where T : new();
        List<Dictionary<string, object?>> ExecuteNoFormat(string sql, Dictionary<string, object>? bindParams, DbConnection connection);
        bool ExecuteNoResponse(string sql, Dictionary<string, object>? bindParams, DbConnection connection);

        void enableLog(bool log);
    }
}
