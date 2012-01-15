using System.Collections.Generic;
using System.Windows;
using LitTravData.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace LitTravProj.Views.Item
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window,INotifyPropertyChanged
    {
        LittleTravellerDataContext context;
        List<string> _seasons;

        public List<string> Seasons
        {
            get { return _seasons; }
            set
            {
                _seasons = value;
                NotifyProprtyChanged("Seasons");
            }
        }

        public AddItem()
        {
            context = new LittleTravellerDataContext();
            Seasons = new List<string>();

            InitializeComponent();

            var sc = from n in context.Seasons select n.SeasonCode;
            Seasons = sc.ToList();
            Seasons.Add("moo");
           // seasonIDComBox.ItemsSource = Seasons;
            // set up first dropdown data

        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyProprtyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
