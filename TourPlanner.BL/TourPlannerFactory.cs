using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.BL
{
    /// <summary>
    /// TourPlannerFactory class that serves as a Singleton to only have a single instance of a class. 
    /// </summary>
    public static class TourPlannerFactory
    {
        private static ITourPlannerFactory instance;

        public static ITourPlannerFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new TourPlannerFactoryImpl();
            }
            return instance;
        }
    }
}
