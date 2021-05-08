using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using TourPlanner.UI.Views;
using TourPlanner.Models;
using TourPlanner.BL;

namespace TourPlanner.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Instances
        private ObservableCollection<Tour> tours;
        private ObservableCollection<TourLog> logs;
        private Tour currentItem;
        private string searchCommand;
        private ICommand randomGenerateItemCommand;
        private ICommand randomGenerateLogCommand;
        private ICommand popUpAdd;

        private ITourPlannerFactory tourPlannerFactory;
        public ICommand PopUpAdd => popUpAdd ??= new RelayCommand(OpenAddTourWindow);
        public ICommand RandomGenerateItemCommand => randomGenerateItemCommand ??= new RelayCommand(RandomGenerateItem);
        public ICommand RandomGenerateLogCommand => randomGenerateItemCommand ??= new RelayCommand(RandomGenerateLog);

        private void RandomGenerateItem(object commandParameter)
        {
            Tour generatedItem = tourPlannerFactory.CreateTour("", "", "", "", "", 0);
            tours.Add(generatedItem);
        }
        private void RandomGenerateLog(object commandParameter)
        {
            TourLog generatedLog = tourPlannerFactory.CreateTourLog(CurrentItem, "", "", "", 0, 0, 0, 0, 0, 0, 0);
        }

        #endregion

        #region Properties
        public IEnumerable<Tour> MyFilteredItems
        {
            get
            {
                if (searchCommand == null) return tours;

                return tours.Where(x => x.Name.ToUpper().StartsWith(searchCommand.ToUpper()));
            }
        }
        public ObservableCollection<Tour> Tours { get => tours; set => tours = value; }
        public ObservableCollection<TourLog> Logs { get => logs; set => logs = value; }
        public Tour CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(currentItem));
                }
            }
        }
        public string SearchCommand
        {
            get
            {
                return searchCommand;
            }
            set
            {
                if (searchCommand != value)
                {
                    searchCommand = value;
                    RaisePropertyChangedEvent("SearchText");
                    RaisePropertyChangedEvent("MyFilteredItems");
                }
            }
        }




        #endregion

        #region Constructor
        public MainViewModel()
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
            tours = new ObservableCollection<Tour>();
            logs = new ObservableCollection<TourLog>();
            foreach (Tour item in this.tourPlannerFactory.GetTours())
            {
                tours.Add(item);
            }

        }
        #endregion

        #region Methods
        private void OpenAddTourWindow(object commandParameter)
        {
            AddTourWindow atw = new AddTourWindow();
            atw.DataContext = new AddTourViewModel();
            atw.ShowDialog();
        }
        #endregion

    }
}
