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

        public TourPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
        }
        public TourPostgresDAO(IDatabase database)
        {
            this.database = database;
        }
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

        public IEnumerable<Tour> GetTours()
        {
            DbCommand toursCommand = database.CreateCommand(SQL_GET_ALL_ITEMS);
            return QueryToursFromDatabase(toursCommand);
        }
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
