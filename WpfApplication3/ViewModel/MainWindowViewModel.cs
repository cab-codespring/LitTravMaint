using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LitTravProj.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        List<string> _seasons;

        public MainWindowViewModel()
        {
            Seasons = new List<string>() { "Spring 12", "Summer 12", "Fall 12" }; // put some sample data in there
        }


        public List<string> Seasons
        {
            get
            {
                return _seasons;
            }
            set
            {
                _seasons = value;
                NotifyProprtyChanged("Seasons");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyProprtyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
