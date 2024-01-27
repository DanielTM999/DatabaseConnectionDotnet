using DatabaseConnection.conn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.core
{
    public interface IConnection
    {
        public delegate void Config(ConnectionFactory factory);

        public DbConnection GenerateConnection(Config config);

    }
}
