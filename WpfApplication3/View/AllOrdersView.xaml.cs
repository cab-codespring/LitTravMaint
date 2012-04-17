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
using LitTravProj.Reports;

namespace LitTravProj.View
{
    /// <summary>
    /// Interaction logic for AllOrdersView.xaml
    /// </summary>
    public partial class AllOrdersView : UserControl
    {
        public AllOrdersView()
        {
            InitializeComponent();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderInvoiceForm oiv = new OrderInvoiceForm();
            oiv.Show();

        }
    }
}
