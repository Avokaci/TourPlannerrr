using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TourPlanner.DAL.Common
{
    /// <summary>
    /// Interface that creates the method heads for the interaction with the database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Method to create a command that is to be executed in the database. 
        /// </summary>
        /// <param name="genericCommandString"></param>
        /// <returns></returns>
        DbCommand CreateCommand(string genericCommandString);
        /// <summary>
        /// Method to declare the Parameters of a database command. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int DeclareParameter(DbCommand command, string name, DbType type);

        /// <summary>
        /// Method to define the parameters of a database command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        void DefineParameter(DbCommand command, string name, DbType type, object value);
        /// <summary>
        /// Method to set the parameters of a database command-
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetParameter(DbCommand command, string name, object value);
        /// <summary>
        /// Method to execute a database command via Executereader. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(DbCommand command);
        /// <summary>
        /// Method to execute a database command via ExecuteScalar. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int ExecuteScalar(DbCommand command);
    }
}
