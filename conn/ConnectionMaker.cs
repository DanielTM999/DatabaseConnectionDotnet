#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using DatabaseConnection.conn.exceptions;
using DatabaseConnection.core;
using System.Data.Common;
using System.Reflection;

namespace DatabaseConnection.conn
{
    public class ConnectionMaker : IConnection
    {

        public DbConnection GenerateConnection(IConnection.Config config)
        {
            ConnectionFactory factory = new ConnectionFactory();
            config(factory);
            return ConnetionMaker(factory);
        }

        private DbConnection ConnetionMaker(ConnectionFactory factory)
        {
            string sqlConnectionString = factory.StringConnection();
            string nomeDaClasse;
            Type tipoDaClasse;
            DbConnection connection;

            switch (factory.DbType)
            {
                case DbTypes.SqlServer:
                    nomeDaClasse = "Microsoft.Data.SqlClient.SqlConnection";
                    Assembly assemblySqlServer = CarregarAssemblyComVerificacao("Microsoft.Data.SqlClient");
                    tipoDaClasse = assemblySqlServer.GetType(nomeDaClasse);
                    break;

                case DbTypes.PostgreSql:
                    nomeDaClasse = "Npgsql.NpgsqlConnection";
                    Assembly assemblyNpgsql = CarregarAssemblyComVerificacao("Npgsql");
                    tipoDaClasse = assemblyNpgsql?.GetType(nomeDaClasse);
                    break;

                case DbTypes.Mysql:
                    nomeDaClasse = "MySql.Data.MySqlClient.MySqlConnection";
                    Assembly assemblyMySql = CarregarAssemblyComVerificacao("MySql.Data");
                    tipoDaClasse = assemblyMySql?.GetType(nomeDaClasse);
                    break;

                default:
                    throw new ConnectionException("Banco de dados Não suportado");
            }

            connection = (DbConnection)Activator.CreateInstance(tipoDaClasse, sqlConnectionString);

            connection.Open();
            return connection;
        }

        private Assembly CarregarAssemblyComVerificacao(string nomeDoAssembly)
        {
            try
            {
                return Assembly.Load(nomeDoAssembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar o assembly {nomeDoAssembly}: {ex.Message}");
                return null;
            }
        }

    }
}
