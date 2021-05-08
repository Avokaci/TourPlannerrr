﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.UI.Views
{
    /// <summary>
    /// Interaction logic for AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public AddTourWindow()
        {
            InitializeComponent();
            this.DataContext = new AddTourViewModel();
        }
    }
}
