using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitTravData.Model;

namespace LitTravProj.ViewModel
{
    class ItemAddedEventArgs
    {
         public ItemAddedEventArgs(Item newItem)
        {
            this.NewItem = newItem;
        }

        public Item NewItem { get; private set; }
    }
}
