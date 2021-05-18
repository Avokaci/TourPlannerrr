using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using TourPlanner.DAL.Common;
using TourPlanner.DAL.DAO;
using TourPlanner.Models;

namespace TourPlanner.DAL.PostgreSQLServer
{
    /// <summary>
    /// TourPostgresDAO class that is responsible for database interaction regarding tours. 
    /// </summary>
    public class TourPostgresDAO : ITourDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"Tour\" WHERE \"id\" = @id;";
        private const string SQL_GET_ALL_ITEMS = "SELECT * FROM public.\"Tour\";";
        private const string SQL_INSERT_NEW_ITEM = "INSERT INTO public.\"Tour\"" +
            "(name, description, \"from\", \"to\", \"routeInformation\", distance) " +
            "VALUES(@name, @description, @from, @to, @routeInformation, @distance) " +
            "RETURNING \"id\";";


        private IDatabase database;
        /// <summary>
        /// Constructor of class that takes no arguments. 
        /// </summary>
        public TourPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
        }
        /// <summary>
        /// Constructor of class that takes database object as a parameter. 
        /// </summary>
        /// <param name="database"></param>
        public TourPostgresDAO(IDatabase database)
        {
            this.database = database;
        }
        /// <summary>
        /// Method that is responsible for inserting a tour item in the database. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Tour AddNewItem(string name, string description, string from, string to, string routeInformation, int distance)
        {

            DbCommand insertCommand = null;
            try
            {
                insertCommand = database.CreateCommand(SQL_INSERT_NEW_ITEM);
                database.DefineParameter(insertCommand, "@name", DbType.String, name);
                database.DefineParameter(insertCommand, "@description", DbType.String, description);
                database.DefineParameter(insertCommand, "@from", DbType.String, from);
                database.DefineParameter(insertCommand, "@to", DbType.String, to);
                database.DefineParameter(insertCommand, "@routeInformation", DbType.String, routeInformation);
                database.DefineParameter(insertCommand, "@distance", DbType.Int32, distance);
                log.Info("Tour " + name +" succesfully added in database");

            }
            catch (Exception ex)
            {
                log.Error("Could not add tour " + name + " to database " + ex.Message);
            }
            return FindById(database.ExecuteScalar(insertCommand));
        }
        /// <summary>
        /// Method that is responsible for creating a copy of a tour in the database. 
        /// </summary>
        /// <param name="tourReference"></param>
        /// <returns></returns>
        public Tour CopyItem(Tour tourReference)
        {
            DbCommand insertCommand = null;
            try
            {
                insertCommand = database.CreateCommand(SQL_INSERT_NEW_ITEM);
                database.DefineParameter(insertCommand, "@name", DbType.String, tourReference.Name);
                database.DefineParameter(insertCommand, "@description", DbType.String, tourReference.Description);
                database.DefineParameter(insertCommand, "@from", DbType.String, tourReference.From);
                database.DefineParameter(insertCommand, "@to", DbType.String, tourReference.To);
                database.DefineParameter(insertCommand, "@routeInformation", DbType.String, tourReference.RouteInformation);
                database.DefineParameter(insertCommand, "@distance", DbType.Int32, tourReference.Distance);
                log.Info("Tour succesfully copied from database");

            }
            catch (Exception ex)
            {
                log.Error("Could not copy tour " + tourReference.Name + " in database " + ex.Message);
            }
            return FindById(database.ExecuteScalar(insertCommand));
        }
        /// <summary>
        /// Method that finds a tour by id from the database. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tour FindById(int id)
        {
            DbCommand findCommand = null;
            IEnumerable<Tour> tourList = null;
            try
            {
                findCommand = database.CreateCommand(SQL_FIND_BY_ID);
                database.DefineParameter(findCommand, "@id", DbType.Int32, id);
                tourList = QueryToursFromDatabase(findCommand);
                log.Info("Tour found by ID in database " + id);

            }
            catch (Exception ex)
            {
                log.Error("Could not find tour by id" + id + " in database " + ex.Message);
            }
            return tourList.FirstOrDefault();
        }
        /// <summary>
        /// Method that returns a list of all the tours in the database. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tour> GetTours()
        {
            DbCommand toursCommand = database.CreateCommand(SQL_GET_ALL_ITEMS);
            return QueryToursFromDatabase(toursCommand);
        }
        /// <summary>
        /// Method that adds tours from the database into the local list of tours. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private IEnumerable<Tour> QueryToursFromDatabase(DbCommand command)
        {
            List<Tour> tourList = new List<Tour>();
            try
            {
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        tourList.Add(new Tour(
                            (int)reader["id"],
                            (string)reader["name"],
                            (string)reader["description"],
                            (string)reader["from"],
                            (string)reader["to"],
                            (string)reader["routeInformation"],
                            (int)reader["distance"]
                            ));
                    }
                }
                log.Info("Succesfully queried tours from database");
            }
            catch (Exception ex)
            {
                log.Error("Could not query tours from database " + ex.Message);
            }

            return tourList;
        }
    }
}
