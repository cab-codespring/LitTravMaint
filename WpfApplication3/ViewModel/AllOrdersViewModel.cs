using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitTravData.Model;
using System.Data;
using System.Windows.Controls;

namespace LitTravProj.ViewModel
{

    class AllOrdersViewModel : WorkspaceViewModel
    {
        LittleTravellerDataContext context;

        public AllOrdersViewModel()
        {
            context = new LittleTravellerDataContext();
            this.DisplayName = "View All Orders";
            Orders = context.OrderViews.ToList<OrderView>();
        }
        public List<OrderView> Orders { get; private set; }


    }
}
