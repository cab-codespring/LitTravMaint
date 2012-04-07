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
            ClearFieldsCommand = new ReactiveCommand();
            ClearFieldsCommand.Subscribe(_ => ClearFields());
            BillToStateOptions = context.states.ToList();
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
             get 
             { 
                 return _selectedCustomer; 
             }
             set
            {
             _selectedCustomer = value;
            }

        }


        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }

            set
            {

                _customerName = value;
                if (_customerName.Length == 0)
                    return;
                Customer existingCustomer = null;
                // first or default returns null if none
                existingCustomer = context.Customers.FirstOrDefault(it => it.CompanyName.Equals(_customerName));
                if (existingCustomer == null)
                {
                    //item doesn;t exist              
                    _customer = new Customer();
                    CustomerNum = GetNewCustomerNum().ToString();
                    _customerExists = false;
                    return; // new customer
                }
                _customer = existingCustomer;
                _customerExists = true;

                CustomerNum = _customer.CustomerNum.ToString();
                CompanyName = _customer.CompanyName;
                BillToAddr1 = _customer.BillToAddr1;
                BillToAddr2 = _customer.BillToAddr2;
                BillToCity = _customer.BillToCity;
                SelectedBillToState.Name = _customer.BillToState;
                BillToZip5 = _customer.BillToZip5;
                BillToZip4 = _customer.BillToZip4;
                BillToPhone = _customer.BillToPhone;
                BillToFax = _customer.BillToFax;
                Email = _customer.email;
                ShipToAddr1 = _customer.ShipToAddr1;
                ShipToAddr2 = _customer.ShipToAddr2;
                ShipToCity = _customer.ShipToCity;
                SelectedShipToState = _customer.ShipToState;
                ShipToZip5 = _customer.ShipToZip5;
                ShipToZip4 = _customer.ShipToZip4;
                ShipToPhone = _customer.ShipToPhone;
                ShipToFax = _customer.ShipToFax;

            }
        }

        private short GetNewCustomerNum()
        {
            return (short) ((from Customer in context.Customers select Customer.CustomerNum).Max() + 1); 
        }

        private bool ValidateFields()
        {
            if ((BillToAddr1 == null && BillToAddr2 == null) ||
                BillToCity == null ||
                SelectedBillToState == null ||
                BillToPhone == null || 
                BillToZip5 == null ||
                CustomerNum.Length == 0 ||
                (BillToAddr1.Length == 0 && BillToAddr2.Length == 0) || 
                SelectedBillToState.Name.Length == 0 ||
                BillToPhone.Length == 0 || 
                BillToZip5.Length == 0 )
                return false;
             return true;
        }

        public ReactiveCommand SaveCommand { get; private set; }
        private void SaveItem()
        {
            _customer.CompanyName = CompanyName;
            _customer.BillToAddr1 = BillToAddr1;
            _customer.BillToAddr2 = BillToAddr2;
            _customer.BillToCity = BillToCity;
            _customer.BillToState = SelectedBillToState.Name;
            _customer.BillToZip5 = BillToZip5;
            _customer.BillToZip4 = BillToZip4;
            _customer.BillToState = SelectedBillToState.Name;
            _customer.BillToPhone = BillToPhone;
            _customer.BillToFax = BillToFax;
            _customer.email = Email;
            _customer.ShipToAddr1 = ShipToAddr1;
            _customer.ShipToAddr2 = ShipToAddr2;
            _customer.ShipToCity = ShipToCity;
            _customer.ShipToState = SelectedShipToState;
            _customer.ShipToZip5 = ShipToZip5;
            _customer.ShipToZip4 = ShipToZip4;
            _customer.ShipToPhone = ShipToPhone;
            _customer.ShipToFax = ShipToFax;
            if (!_customerExists)
                context.Customers.InsertOnSubmit(_customer);
            context.SubmitChanges();
            ClearFields();
 
        }

        public ReactiveCommand ClearFieldsCommand { get; private set; }
        public void ClearFields()
        {
            this.RaiseAndSetIfChanged(vm => vm.CustomerNum, ref _customerNum, "");
            this.RaiseAndSetIfChanged(vm => vm.CompanyName, ref _companyName, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToAddr1, ref _billToAddr1, "");
            this.RaiseAndSetIfChanged(vm => vm.CustomerNum, ref _customerNum, "");
            this.RaiseAndSetIfChanged(vm => vm.CompanyName, ref _companyName, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToAddr1, ref _billToAddr1, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToAddr2, ref _billToAddr2, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToCity, ref _billToCity, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToZip5, ref _billToZip5, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToPhone, ref _billToPhone, "");
            this.RaiseAndSetIfChanged(vm => vm.BillToFax, ref _billToFax, "");
            this.RaiseAndSetIfChanged(vm => vm.Email, ref _email, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToAddr1, ref _shipToAddr1, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToAddr2, ref _shipToAddr2, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToCity, ref _shipToCity, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToZip5, ref _shipToZip5, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToPhone, ref _shipToPhone, "");
            this.RaiseAndSetIfChanged(vm => vm.ShipToFax, ref _shipToFax, "");
            
        }
        

        public List<Customer> Customers { get; private set; }

        private string _customerNum;
        public string CustomerNum
        {
            get
            {
                if (_customer == null)
                    return "";
                _customerNum = _customer.CustomerNum.ToString();
                return _customerNum;
            }
            set
            {
             
                _customer.CustomerNum = Int16.Parse(value);
                this.RaiseAndSetIfChanged(vm => vm.CustomerNum, ref _customerNum, value);
            }
        }


        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.CompanyName, ref _companyName, value);
            }

        }
        private string _billToAddr1;
        public string BillToAddr1
        {
            get { return _billToAddr1; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToAddr1, ref _billToAddr1, value);
            }
        }

        private string _billToAddr2;
        public string BillToAddr2
        {
            get { return _billToAddr2; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToAddr2, ref _billToAddr2, value);
            }

        }

        private string _billToCity;
        public string BillToCity
        {
            get { return _billToCity; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToCity, ref _billToCity, value);
            }

        }

        /// <summary>
        /// Returns a list of states used to populate the BillToState selector.
        /// </summary>
        public IEnumerable<state> BillToStateOptions { get; private set; }

        private state _selectedBillToState = new state();
        public state SelectedBillToState
        {
            get { return _selectedBillToState; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedBillToState, ref _selectedBillToState, value);
            }
        }

        private string _billToZip5;
        public string BillToZip5
        {
            get { return _billToZip5; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToZip5, ref _billToZip5, value);
            }

        }


        private string _billToZip4;
        public string BillToZip4
        {
            get { return _billToZip4; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToZip4, ref _billToZip4, value);
            }

        }


        private string _billToPhone;
        public string BillToPhone
        {
            get { return _billToPhone; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToPhone, ref _billToPhone, value);
            }

        }


        private string _billToFax;
        public string BillToFax
        {
            get { return _billToPhone; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.BillToFax, ref _billToFax, value);
            }

        }


        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.Email, ref _email, value);
            }

        }


        private string _shipToAddr1;
        public string ShipToAddr1
        {
            get { return _shipToAddr1; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToAddr1, ref _shipToAddr1, value);
            }

        }

        private string _shipToAddr2;
        public string ShipToAddr2
        {
            get { return _shipToAddr2; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToAddr2, ref _shipToAddr2, value);
            }

        }

        private string _shipToCity;
        public string ShipToCity
        {
            get { return _shipToCity; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToCity, ref _shipToCity, value);
            }

        }

        private string _selectedShipToState;
        public string SelectedShipToState
        {
            get { return _selectedShipToState; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedShipToState, ref _selectedShipToState, value);
            }
        }

        private string _shipToZip5;
        public string ShipToZip5
        {
            get { return _shipToZip5; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToZip5, ref _shipToZip5, value);
            }

        }


        private string _shipToZip4;
        public string ShipToZip4
        {
            get { return _shipToZip4; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToZip4, ref _shipToZip4, value);
            }

        }


        private string _shipToPhone;
        public string ShipToPhone
        {
            get { return _shipToPhone; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToPhone, ref _shipToPhone, value);
            }

        }


        private string _shipToFax;
        public string ShipToFax
        {
            get { return _shipToPhone; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ShipToFax, ref _shipToFax, value);
            }

        }


    }
}