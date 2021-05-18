using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using TourPlanner.DAL.Common;

namespace TourPlanner.DAL.PostgreSQLServer
{
    /// <summary>
    /// Database class, which is responsible for all the interaction with the database. This class is responsible for creating the connection, commands, declare, defines 
    /// and sets the parameters for a command and executes the commands. 
    /// 
    /// </summary>
    public class Database : IDatabase
    {
        private string connectionString;
        /// <summary>
        /// Constructor for database class, which has the connectionstring as a parameter.
        /// </summary>
        /// <param name="connectionString"></param>
        public Database(string connectionString)
        {
            this.connectionString = connectionString;

        }
        /// <summary>
        /// Method that creates the connection to the database
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateConnection()
        {
            DbConnection connection = new NpgsqlConnection(this.connectionString);
            connection.Open();

            return connection;
        }
        /// <summary>
        /// Method that is responsible for the creation of a command. 
        /// </summary>
        /// <param name="genericCommandString"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string genericCommandString)
        {
            return new NpgsqlCommand(genericCommandString);
        }
        /// <summary>
        /// Method that is responsible for declaring the parameters of a command. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Method that defines parameters of a command. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int index = DeclareParameter(command, name, type);
            command.Parameters[index].Value = value;
        }
        /// <summary>
        /// Method that sets parameters of a command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
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
        /// <summary>
        /// Method that executes a command via ExececuteReader command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand command)
        {
            command.Connection = CreateConnection();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// Method that executes a command via ExecuteScalar command. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteScalar(DbCommand command)
        {

            command.Connection = CreateConnection();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
