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
    public class LogPostgresDAO : ILogDAO
    {
        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"TourLog\" WHERE \"id\" = @id;";
        private const string SQL_FIND_BY_TOUR_ID = "SELECT * FROM public.\"TourLog\" WHERE \"tourid\" = @tourId;";
        private const string SQL_INSERT_NEW_LOG = "INSERT INTO public.\"TourLog\"" +
            "(tourid, date, totaltime, report,  distance,  rating, averagespeed,  maxspeed,  minspeed,  averagestepcount,  burntcalories)" +
            "VALUES(@tourId, @date, @totalTime, @report, @distance, @rating, @averageSpeed,@maxSpeed,@minSpeed,@averageStepCount,@burntCalories);";

        private IDatabase database;
        private DAO.ITourDAO tourDAO;
        public LogPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
            this.tourDAO = DALFactory.CreateTourItemDAO();
        }
        public LogPostgresDAO(IDatabase database, DAO.ITourDAO tourDAO)
        {
            this.database = database;
            this.tourDAO = tourDAO;
        }
        public TourLog AddNewItemLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            DbCommand insertCommand = database.CreateCommand(SQL_INSERT_NEW_LOG);
            database.DefineParameter(insertCommand, "@tourId", DbType.Int32, tourLogItem.Id);
            database.DefineParameter(insertCommand, "@date", DbType.String, date);
            database.DefineParameter(insertCommand, "@totalTime", DbType.String, totalTime);
            database.DefineParameter(insertCommand, "@report", DbType.String, report);
            database.DefineParameter(insertCommand, "@distance", DbType.Int32, distance);
            database.DefineParameter(insertCommand, "@rating", DbType.Int32, rating);
            database.DefineParameter(insertCommand, "@averageSpeed", DbType.Int32, averageSpeed);
            database.DefineParameter(insertCommand, "@maxSpeed", DbType.Int32, maxSpeed);
            database.DefineParameter(insertCommand, "@minSpeed", DbType.Int32, minSpeed);
            database.DefineParameter(insertCommand, "@averageStepCount", DbType.Int32, averageStepCount);
            database.DefineParameter(insertCommand, "@burntCalories", DbType.Int32, burntCalories);

            return FindById(database.ExecuteScalar(insertCommand));
        }
        public TourLog AddNewItemLog(TourLog log)
        {
            return AddNewItemLog(log.TourLogItem, log.Date, log.TotalTime, log.Report, log.Distance, log.Rating, log.AverageSpeed,
                log.MaxSpeed, log.MinSpeed, log.AverageStepCount, log.BurntCalories);
        }
        public TourLog FindById(int id)
        {
            DbCommand findCommand = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findCommand, "@id", DbType.Int32, id);

            IEnumerable<TourLog> logList = QueryLogsFromDb(findCommand);
            return logList.FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogsForTour(Tour tourItem)
        {
            DbCommand getLogsCommand = database.CreateCommand(SQL_FIND_BY_TOUR_ID);
            database.DefineParameter(getLogsCommand, "@tourId", DbType.Int32, tourItem.Id);

            return QueryLogsFromDb(getLogsCommand);
        }

        private IEnumerable<TourLog> QueryLogsFromDb(DbCommand command)
        {
            List<TourLog> logList = new List<TourLog>();
            using (IDataReader reader = database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    logList.Add(new TourLog(
                        (int)reader["id"],
                        tourDAO.FindById((int)reader["tourId"]),
                        (string)reader["date"],
                        (string)reader["totalTime"],
                        (string)reader["report"],
                        (int)reader["distance"],
                        (int)reader["rating"],
                        (int)reader["averageSpeed"],
                        (int)reader["maxSpeed"],
                        (int)reader["minSpeed"],
                        (int)reader["averageStepCount"],
                        (int)reader["burntCalories"])
                        );
                }
            }
            return logList;
        }
    }
}
