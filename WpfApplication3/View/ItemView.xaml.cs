using System.Windows.Controls;

namespace LitTravProj.View
{
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl
    {
         public ItemView()
        {
            InitializeComponent();
        }
    }
}
    //    LittleTravellerDataContext context;
    //    List<string> _seasons;

    //    public List<string> Seasons
    //    {
    //        get { return _seasons; }
    //        set
    //        {
    //            _seasons = value;
    //            NotifyProprtyChanged("Seasons");
    //        }
    //    }

    //    public ItemView()
    //    {
    //        context = new LittleTravellerDataContext();
    //        Seasons = new List<string>();

    //        InitializeComponent();

    //        var sc = from n in context.Seasons select n.SeasonCode;
    //        Seasons = sc.ToList();
    //        Seasons.Add("moo");
    //       // seasonIDComBox.ItemsSource = Seasons;
    //        // set up first dropdown data

    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    void NotifyProprtyChanged(string propName)
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    //    }

