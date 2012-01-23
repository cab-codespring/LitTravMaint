using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitTravData.Model;
using System.Data;
using System.Windows.Controls;

namespace LitTravProj.ViewModel
{

    class AllItemsViewModel : WorkspaceViewModel
    {  
        LittleTravellerDataContext context;

       public  AllItemsViewModel()
        {
             context = new LittleTravellerDataContext();
        }
        ListViewItem AllItemsGridSource 
        {
            get
            {

                ListView listView1 = new ListView();
                var itemQuery1 =
                    from item in context.Items
                    select item;

                ListViewItem ItemsSource = new ListViewItem();
                ItemsSource = (ListViewItem)itemQuery1;
                return ItemsSource;
            }

        }
    }
}
