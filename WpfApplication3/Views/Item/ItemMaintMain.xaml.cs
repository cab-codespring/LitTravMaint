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
using System.Windows.Shapes;
using LitTravProj.Views.Item;

namespace LitTravProj.Views
{
    /// <summary>
    /// Interaction logic for ItemMaintMain.xaml
    /// </summary>
    public partial class ItemMaintMain : Window
    {
        public ItemMaintMain()
        {
            InitializeComponent();
        }

        private void buttonAddItem_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AddItem();
            newWindow.Show();
        }
    }
}
