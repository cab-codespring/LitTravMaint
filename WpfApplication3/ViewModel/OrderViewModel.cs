using System;
using System.ComponentModel;
using System.Windows.Input;
using LitTravData.Model;
using System.Text.RegularExpressions;
using LitTravProj.Properties;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Collections.ObjectModel;
using System.Globalization;

namespace LitTravProj.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for an Item object.
    /// </summary>
    public class OrderViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private LittleTravellerDataContext context;

        private Order _order;
        private bool _orderExists = false;


        public OrderViewModel()
        {

            this.DisplayName = "Add/Edit Order";

            context = new LittleTravellerDataContext();
            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);
            SaveCommand.Subscribe(_ => SaveOrder());
            NewOrderNumCommand = new ReactiveCommand();
            NewOrderNumCommand.Subscribe(_ => NewOrderNum());

            CustomerOptions = (from cs in context.Customers select cs.CompanyName).ToList();
            SeasonOptions = context.Seasons.ToList();
            SizeTypeOptions = context.SizeTypes.ToList();
        }

        public ReactiveCommand SaveCommand { get; private set; }

        private void SaveOrder()
        {



            if (!_orderExists)
                context.Orders.InsertOnSubmit(_order);
            context.SubmitChanges();

            this.RaiseAndSetIfChanged(vm => vm.OrderNum, ref _orderNum, "");
        }

        private bool ValidateFields()
        {
            return true;
        }

        private string _orderNum = "";
        public string OrderNum
        {
            get { return _orderNum; }

            set
            {
                _orderNum = value;
                if (_orderNum.Length == 0)
                    return;
                Order existingOrder = null;
                // first or default returns null if none
                existingOrder = context.Orders.FirstOrDefault(it => it.OrderNum.Equals(_orderNum));
                if (existingOrder == null)
                {
                    //item doesn;t exist              
                    _order = new Order();
                    _order.OrderNum = Int16.Parse(_orderNum);
                    _orderExists = false;
                    return; // new sku
                }
                _order = existingOrder;
                _orderExists = true;

             
            }
        }

          public ReactiveCommand NewOrderNumCommand { get; private set; }
          public void  NewOrderNum()
        {

            string _newOrderNum = ((from order in context.Orders select order.OrderNum).Max() + 1).ToString();
            this.RaiseAndSetIfChanged(vm => vm.OrderNum, ref _orderNum, _newOrderNum );
            
        }


          /// <summary>
          /// Returns a list of strings used to populate the Customer selector.
          /// </summary>
          public IEnumerable<string> CustomerOptions { get; private set; }

          private string _selectedCustomer;

          public string SelectedCustomer
          {
              get { return _selectedCustomer; }
              set
              { 
                  SelectedCustomerNum = (from cn in context.Customers where cn.CompanyName == _selectedCustomer select cn.CustomerNum).FirstOrDefault();
                  this.RaiseAndSetIfChanged(vm => vm.SelectedCustomer, ref _selectedCustomer, value);
              }
          }


          private short _selectedCustomerNum;

          public short SelectedCustomerNum
          {
              get { return _selectedCustomerNum; }
              set
              {
                  this.RaiseAndSetIfChanged(vm => vm.SelectedCustomerNum, ref _selectedCustomerNum, value);
              }
          }
          /// <summary>
          /// Returns a list of Seasons used to populate the Season selector.
          /// </summary>
          public IEnumerable<Season> SeasonOptions { get; private set; }

          private Season _selectedSeason = new Season();
          public Season SelectedSeason
          {
              get { return _selectedSeason; }
              set
              {
                  this.RaiseAndSetIfChanged(vm => vm.SelectedSeason, ref _selectedSeason, value);
              }
          }

          public IEnumerable<SizeType> SizeTypeOptions { get; private set; }
          private SizeType _selectedSizeTypeID;

          /// <summary>
          /// Size must be limited to this size Type
          /// </summary>

          public SizeType SelectedSizeTypeID
          {

              get { return _selectedSizeTypeID; }
              set
              {
                  // _selectedSizeTypeID = value;
                  this.RaiseAndSetIfChanged(vm => vm.SelectedSizeTypeID, ref _selectedSizeTypeID, value);
      
               }
          }
    }
}
