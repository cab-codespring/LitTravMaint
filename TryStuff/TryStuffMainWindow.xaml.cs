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
using System.Collections.ObjectModel;

namespace TryStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TryStuffMainWindow : Window
    {
        ObservableCollection<Employee> _listOfEmployees = new ObservableCollection<Employee>();

        public ObservableCollection<Employee> ListOfEmployees
        {
            get { return _listOfEmployees; }
            set { _listOfEmployees = value; }
        }

        public TryStuffMainWindow()
        {
            InitializeComponent();

            //this.DataContext = this; // this is important


            InitListOfEmployees();

        }

        void InitListOfEmployees()
        {
            ListOfEmployees.Add(new Employee("Chris","Bennet",200));
            ListOfEmployees.Add(new Employee("Chrissy","Burnham",100));
            ListOfEmployees.Add(new Employee("GoldenBoy","Burnham",300));

        }
    }
}
