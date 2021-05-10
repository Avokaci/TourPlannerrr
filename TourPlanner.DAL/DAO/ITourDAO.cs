using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.DAO
{
    public interface ITourDAO
    {
        Tour FindById(int id);

        Tour AddNewItem(string name, string description, string from, string to, string routeInformation, int distance);
        IEnumerable<Tour> GetTours();
    }
}
