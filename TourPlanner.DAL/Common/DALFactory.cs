using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using TourPlanner.DAL.DAO;

namespace TourPlanner.DAL.Common
{
    /// <summary>
    /// DALFactory class that is responsible for how the objects, in this case tour and tourlogs should be treated e.g. save them in database or in filesystem. 
    /// Information as how to handle the objects are retrieved from the Config file. 
    /// </summary>
    public class DALFactory
    {
        private static string assemblyName;
        private static Assembly AssemblyObject;
        private static IDatabase database;
        private static IFileAccess fileAccess;
        private static bool useFileSystem;

        /// <summary>
        /// Constructor or DALFactory class that decides how to handle the objects based on the configuration in the Settings file. 
        /// </summary>
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
        /// <summary>
        /// Singleton of Database
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            if (database == null)
            {
                database = CreateDatabase();
            }

            return database;
        }
        /// <summary>
        /// Method that creates the Database connection with the connectionString provided in the Config file. 
        /// </summary>
        /// <returns></returns>
        private static IDatabase CreateDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresSQLConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }
        /// <summary>
        /// Method that creates the Database connection with the connectionString provided as parameter. 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static IDatabase CreateDatabase(string connectionString)
        {
            string databaseClassName = assemblyName + ".Database";
            Type dbClass = AssemblyObject.GetType(databaseClassName);

            return Activator.CreateInstance(dbClass, new object[] { connectionString }) as IDatabase;
        }
        /// <summary>
        /// Method that creates a tour item according to chosen handling method. 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Method that creates a tour log according to chosen handling method. 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Singleton of Filesystem access. 
        /// </summary>
        /// <returns></returns>
        public static IFileAccess GetFileAccess()
        {
            if (fileAccess == null)
            {
                fileAccess = CreateFileAccess();
            }

            return fileAccess;
        }

        /// <summary>
        /// Method that sets the start folder file path from the connectionString provided in the Config file
        /// </summary>
        /// <returns></returns>
        private static IFileAccess CreateFileAccess()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StartFolderFilePath"].ConnectionString;
            return CreateFileAccess(connectionString);
        }
        /// <summary>
        /// Method that sets the start folder file path from the connectionString provided as a parameter
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static IFileAccess CreateFileAccess(string connectionString)
        {
            string fileClassName = assemblyName + ".FileAccess";
            Type fileClass = AssemblyObject.GetType(fileClassName);

            return Activator.CreateInstance(fileClass, new object[] { connectionString }) as IFileAccess;
        }
    }
}
