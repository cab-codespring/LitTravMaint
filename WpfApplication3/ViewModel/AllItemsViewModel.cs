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

          

           //Items =   (from item in context.ItemsGrisView
           //            join st in context.StyleTypes on item.StyleTypeID equals st.ID
           //            join d in context.Designs on item.DesignID equals d.ID
           //            select
           //                 new
           //                 {
           //                     item.Sku,
           //                     item.SeasonID,
           //                     item.ColorID,
           //                     item.Color2ID,
           //                     item.Color3ID,
           //                     StyleTypeDesc = st.Desc,
           //                     DesignDesc = d.Desc,
           //                     item.Price
           //                 }).Cast<object>().ToList();



             Items = context.ItemsGridView.ToList();
        }
       public List<ItemsGridView> Items { get; private set; }

        //public ListViewItem AllItemsGridSource 
        //{
        //    get
        //    {

        //        ListView listView1 = new ListView();
        //        var itemQuery1 =
        //            from item in context.Items
        //            select item;

        //        ListViewItem ItemsSource = new ListViewItem();
        //        ItemsSource = (ListViewItem)itemQuery1;
        //        return ItemsSource;
        //    }

        //}
    }
}
