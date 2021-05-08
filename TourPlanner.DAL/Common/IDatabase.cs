using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TourPlanner.DAL.Common
{
    public interface IDatabase
    {
        DbCommand CreateCommand(string genericCommandString);
        int DeclareParameter(DbCommand command, string name, DbType type);

        void DefineParameter(DbCommand command, string name, DbType type, object value);
        void SetParameter(DbCommand command, string name, object value);
        IDataReader ExecuteReader(DbCommand command);

        int ExecuteScalar(DbCommand command);
    }
}
