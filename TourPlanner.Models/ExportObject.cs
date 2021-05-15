using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class ExportObject
    {
        private Tour tour;
        private List<TourLog> tourLogs;
        public Tour Tour { get => tour; set => tour = value; }
        public List<TourLog> TourLogs { get => tourLogs; set => tourLogs = value; }
    }
}
