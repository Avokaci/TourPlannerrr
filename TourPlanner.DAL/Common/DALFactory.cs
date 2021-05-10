using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using TourPlanner.DAL.DAO;

namespace TourPlanner.DAL.Common
{
    public class DALFactory
    {
        private static string assemblyName;
        private static Assembly AssemblyObject;
        private static IDatabase database;
        private static IFileAccess fileAccess;
        private static bool useFileSystem;

        static DALFactory()
        {
            useFileSystem = bool.Parse(ConfigurationManager.AppSettings["useFileSystem"]);
            if (useFileSystem)
            {
                assemblyName = ConfigurationManager.AppSettings["DALFileAssembly"];
            }
            else
            {
                assemblyName = ConfigurationManager.AppSettings["DALSqlAssembly"];

            }
            AssemblyObject = Assembly.Load(assemblyName);
        }

        public static IDatabase GetDatabase()
        {
            if (database == null)
            {
                database = CreateDatabase();
            }

            return database;
        }

        private static IDatabase CreateDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresSQLConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        private static IDatabase CreateDatabase(string connectionString)
        {
            string databaseClassName = assemblyName + ".Database";
            Type dbClass = AssemblyObject.GetType(databaseClassName);

            return Activator.CreateInstance(dbClass, new object[] { connectionString }) as IDatabase;
        }

        public static ITourDAO CreateTourItemDAO()
        {
            string className = assemblyName + ".TourPostgresDAO";
            if (useFileSystem)
            {
                className = assemblyName + ".TourFileDAO";
            }
            Type tourType = AssemblyObject.GetType(className);

            return Activator.CreateInstance(tourType) as ITourDAO;
        }
        public static ILogDAO CreateTourLogDAO()
        {
            string className = assemblyName + ".LogPostgresDAO";
            if (useFileSystem)
            {
                className = assemblyName + ".LogFileDAO";
            }
            Type logType = AssemblyObject.GetType(className);

            return Activator.CreateInstance(logType) as ILogDAO;
        }

        public static IFileAccess GetFileAccess()
        {
            if (fileAccess == null)
            {
                fileAccess = CreateFileAccess();
            }

            return fileAccess;
        }

        private static IFileAccess CreateFileAccess()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StartFolderFilePath"].ConnectionString;
            return CreateFileAccess(connectionString);
        }

        private static IFileAccess CreateFileAccess(string connectionString)
        {
            string fileClassName = assemblyName + ".FileAccess";
            Type fileClass = AssemblyObject.GetType(fileClassName);

            return Activator.CreateInstance(fileClass, new object[] { connectionString }) as IFileAccess;
        }
    }
}
