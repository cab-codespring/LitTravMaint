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
             this.DisplayName = "View All Items";
             Items = context.ItemsGridViews.ToList<ItemsGridView>();
        }
       public List<ItemsGridView> Items { get; private set; }

       
    }
}
