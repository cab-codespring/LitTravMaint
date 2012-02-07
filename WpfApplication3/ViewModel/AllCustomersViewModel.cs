using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitTravData.Model;
using System.Data;
using System.Windows.Controls;

namespace LitTravProj.ViewModel
{

    class AllCustomersViewModel : WorkspaceViewModel
    {
        LittleTravellerDataContext context;

        public AllCustomersViewModel()
        {
            context = new LittleTravellerDataContext();
            this.DisplayName = "View All Customers";
            Customers = context.Customers.ToList<Customer>();
        }
        public List<Customer> Customers { get; private set; }


    }
}
