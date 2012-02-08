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

namespace LitTravProj.ViewModel
{
    class CustomerViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private LittleTravellerDataContext context;

        private Customer _customer;
        private bool _customerExists = false;


        public CustomerViewModel()
        {
            context = new LittleTravellerDataContext();

            this.DisplayName = "Add/Edit Customer";
            Customers = context.Customers.ToList<Customer>();

            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);
            SaveCommand.Subscribe(_ => SaveItem());
        }

        private string _customerNum;
        public string SKU
        {
            get { return _customerNum; }

            set
            {
                _customerNum = value;
                if (_customerNum.Length == 0)
                    return;
                Customer existingCustomer = null;
                // first or default returns null if none
                existingCustomer = context.Customers.FirstOrDefault(it => it.CustomerNum.Equals(_customerNum));
                if (existingCustomer == null)
                {
                    //item doesn;t exist              
                    _customer = new Customer();

                    try
                    {
                        _customer.CustomerNum = Int16.Parse(_customerNum);
                    }
                    catch (Exception)
                    {

                        _customerNum = "";
                        return;
                    }
                    _customerExists = false;
                    return; // new sku
                }
                _customer = existingCustomer;
                _customerExists = true;


            }
        }



        private bool ValidateFields()
        {
            throw new NotImplementedException();
        }

        private void SaveItem()
        {
            throw new NotImplementedException();
        }

        public ReactiveCommand SaveCommand { get; private set; }



        public string CompanyName { get;  set; }

        public string BillToAddr1 { get;  set; }

        public string BillToAddr2 { get;  set; }

        public string BillToCity { get;  set; }

        public string BillToState { get;  set; }

        public string BillToZip5 { get;  set; }

        public string BillToZip4 { get;  set; }

        public string BillToPhone { get;  set; }

        public string BillToFax { get;  set; }

        public string Email { get;  set; }

        public string ShipToAddr1 { get;  set; }

        public string ShipToAddr2 { get;  set; }

        public string ShipToCity { get;  set; }

        public string ShipToState { get;  set; }

        public string ShipToZip5 { get;  set; }

        public string ShipToZip4 { get;  set; }

        public string ShipToPhone { get;  set; }

        public string ShipToFax { get; set; }

        public List<Customer> Customers { get; private set; }

    }
}