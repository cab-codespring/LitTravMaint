using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
//using LitTravProj.DataAccess;
using LitTravProj.Properties;

namespace LitTravProj.ViewModel
{
    /// <summary>
    /// Represents a container of ItemViewModel objects
    /// that has support for staying synchronized with the
    /// CustomerRepository.  This class also provides information
    /// related to multiple selected customers.
    /// </summary>
    public class AllCustomersViewModel : WorkspaceViewModel
    {
        #region Fields

     //   readonly CustomerRepository _customerRepository;

        #endregion // Fields

        #region Constructor

        public AllCustomersViewModel() //CustomerRepository customerRepository)
        {
            //if (customerRepository == null)
            //    throw new ArgumentNullException("customerRepository");

            //base.DisplayName = Strings.AllCustomersViewModel_DisplayName;            

            //_customerRepository = customerRepository;

            //// Subscribe for notifications of when a new customer is saved.
            //_customerRepository.CustomerAdded += this.OnCustomerAddedToRepository;

            //// Populate the AllCustomers collection with CustomerViewModels.
            //this.CreateAllCustomers();              
        }

        void CreateAllCustomers()
        {
            //List<ItemViewModel> all =
            //    (from cust in _customerRepository.GetCustomers()
            //     select new ItemViewModel(cust)).ToList();

            //foreach (ItemViewModel cvm in all)
            //    cvm.PropertyChanged += this.OnCustomerViewModelPropertyChanged;

            //this.AllCustomers = new ObservableCollection<ItemViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> AllCustomers { get; private set; }

        /// <summary>
        /// Returns the total sales sum of all selected customers.
        /// </summary>
        //public double TotalSelectedSales
        //{
        //    get
        //    {
        //        return this.AllCustomers.Sum(
        //            custVM => custVM.IsSelected ? custVM.TotalSales : 0.0);
        //    }
        //}

        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ItemViewModel custVM in this.AllCustomers)
                custVM.Dispose();

            this.AllCustomers.Clear();
            this.AllCustomers.CollectionChanged -= this.OnCollectionChanged;

            //_customerRepository.CustomerAdded -= this.OnCustomerAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ItemViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnCustomerViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ItemViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnCustomerViewModelPropertyChanged;
        }

        void OnCustomerViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as ItemViewModel).VerifyPropertyName(IsSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
                this.OnPropertyChanged("TotalSelectedSales");
        }

        void OnCustomerAddedToRepository(object sender, ItemAddedEventArgs e)
        {
          //  var viewModel = new ItemViewModel(e.NewCustomer);
         //   this.AllCustomers.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}