using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.DAO
{
    /// <summary>
    /// Interface that defines the Methods that should be provided for handling Tour DAOs
    /// </summary>
    public interface ITourDAO
    {
        /// <summary>
        /// Method that returns an item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tour FindById(int id);
        /// <summary>
        /// Method that adds a new tour item to the system of your choice. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        Tour AddNewItem(string name, string description, string from, string to, string routeInformation, int distance);
        /// <summary>
        /// Method that returns all Tours from the system of your choice.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tour> GetTours();
    }
}
