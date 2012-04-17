using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LittleTraveller.Models
{
    public class OrderEntryViewModel
    { // use automatic properties
        private string season = null; // never assigned
        public OrderEntryViewModel(Order order, List<Item> items, List<OrderItem> orderItems)
        {
            Order = order;
            Items = items;
            OrderItems = orderItems;
        }

        public Order Order { get; private set; }
        public List<Item> Items { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }
        public string Season
        {
            get
            {
                return season;
            }
        }
    }
}