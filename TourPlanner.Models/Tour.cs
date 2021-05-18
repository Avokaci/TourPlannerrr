using System;

namespace TourPlanner.Models
{
    /// <summary>
    /// Tour item class. Includes class attributes, constructor and access modifiers. 
    /// </summary>
    public class Tour
    {
        private int id;
        private string name;
        private string description;
        private string from;
        private string to;
        private string routeInformation;
        private int distance;

        /// <summary>
        /// Constructor of class which intantiates a Tour item. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        public Tour(int id, string name, string description, string from, string to, string routeInformation, int distance)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.from = from;
            this.to = to;
            this.routeInformation = routeInformation;
            this.distance = distance;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public string RouteInformation { get => routeInformation; set => routeInformation = value; }
        public int Distance { get => distance; set => distance = value; }
    }
}
