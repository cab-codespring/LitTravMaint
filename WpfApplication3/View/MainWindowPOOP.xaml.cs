using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LitTravProj.Views;
using LitTravProj.ViewModels;
using LittleTraveller.Models;

namespace LitTravProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _mainViewModel = null;
        LittleTravellerDataContext _ltData = null;

        public MainWindowViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { _mainViewModel = value; }
        }

        public MainWindow()
        {

            _mainViewModel = new MainWindowViewModel();

            //this.DataContext = _mainViewModel;

            InitializeComponent();

            _ltData = new LittleTravellerDataContext();
            //List<Season> seasonList = _ltData.GetSeasons();
           
            //_mainViewModel.Seasons = _ltData.GetSeasons();

            this.DataContext = _mainViewModel;

        }

      
    }
}
