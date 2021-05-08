using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using TourPlanner.DAL.Common;

namespace TourPlanner.DAL.PostgreSQLServer
{
    public class Database : IDatabase
    {
        private string connectionString;
        public Database(string connectionString)
        {
            this.connectionString = connectionString;

        }

        private DbConnection CreateConnection()
        {
            DbConnection connection = new NpgsqlConnection(this.connectionString);
            connection.Open();

            return connection;
        }
        public DbCommand CreateCommand(string genericCommandString)
        {
            return new NpgsqlCommand(genericCommandString);
        }

        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            if (!command.Parameters.Contains(name))
            {
                int index = command.Parameters.Add(new NpgsqlParameter(name, type));
                return index;
            }
            else
            {
                //Implement unit test to see if it works
                throw new ArgumentException(string.Format("Parameter {0} already exists.", name));
            }
        }

        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int index = DeclareParameter(command, name, type);
            command.Parameters[index].Value = value;
        }

        public void SetParameter(DbCommand command, string name, object value)
        {
            if (command.Parameters.Contains(name))
            {
                command.Parameters[name].Value = value;
            }
            else
            {
                throw new ArgumentException(string.Format("Parameter {0} does not exist.", name));
            }
        }
        public IDataReader ExecuteReader(DbCommand command)
        {
            command.Connection = CreateConnection();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ExecuteScalar(DbCommand command)
        {
            command.Connection = CreateConnection();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
