using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    /// <summary>
    /// Class that defines the structure of an exported item. A tour consists of a list of tourlogs (multiple tourlogs)
    /// </summary>
    public class ExportObject
    {
        private Tour tour;
        private List<TourLog> tourLogs;
        public Tour Tour { get => tour; set => tour = value; }
        public List<TourLog> TourLogs { get => tourLogs; set => tourLogs = value; }
    }
}
