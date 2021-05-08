using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.BL
{
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
