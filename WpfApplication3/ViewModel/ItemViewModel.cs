using System;
using System.ComponentModel;
using System.Windows.Input;
using LitTravData.Model;
using System.Text.RegularExpressions;
using LitTravProj.Properties;
using System.Diagnostics;
using System.Linq;
namespace LitTravProj.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for an Item object.
    /// </summary>
    public class ItemViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Item _item;
        string _season;
        string[] _seasonOptions;
        bool _isSelected;
        RelayCommand _saveCommand;
        LittleTravellerDataContext context;
        #endregion // Fields

        #region Constructor

        public ItemViewModel(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            context = new LittleTravellerDataContext();
            _season = "none";
        }
        public ItemViewModel()
        {
           
            context = new LittleTravellerDataContext();
            _season = "none";
        }
        #endregion // Constructor

        #region Item Properties

        //public string Email
        //{
        //    get { return _customer.Email; }
        //    set
        //    {
        //        if (value == _customer.Email)
        //            return;

        //        _customer.Email = value;

        //        base.OnPropertyChanged("Email");
        //    }
        //}

        //public string FirstName
        //{
        //    get { return _customer.FirstName; }
        //    set
        //    {
        //        if (value == _customer.FirstName)
        //            return;

        //        _customer.FirstName = value;

        //        base.OnPropertyChanged("FirstName");
        //    }
        //}

        //public bool IsCompany
        //{
        //    get { return _customer.IsCompany; }
        //}

        //public string LastName
        //{
        //    get { return _customer.LastName; }
        //    set
        //    {
        //        if (value == _customer.LastName)
        //            return;

        //        _customer.LastName = value;

        //        base.OnPropertyChanged("LastName");
        //    }
        //}

        //public double TotalSales
        //{
        //    get { return _customer.TotalSales; }
        //}

        #endregion // Customer Properties

        #region Presentation Properties

      
         
        /// <summary>
        /// Returns a list of strings used to populate the Customer Type selector.
        /// </summary>
        public string[] SeasonOptions
        {
            get
            {
                var sc = from n in context.Seasons select n.SeasonCode;
                return sc.ToList().ToArray();
                
            }
        }

        //public override string DisplayName
        //{
        //    get
        //    {
        //        if (this.IsNewCustomer)
        //        {
        //            return Strings.CustomerViewModel_DisplayName;
        //        }
        //        else if (_customer.IsCompany)
        //        {
        //            return _customer.FirstName;
        //        }
        //        else
        //        {
        //            return String.Format("{0}, {1}", _customer.LastName, _customer.FirstName);
        //        }
        //    }
        //}

        /// <summary>
        /// Gets/sets whether this customer is selected in the UI.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save()
                      //  param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }

        #endregion // Presentation Properties

        #region Public Methods

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            //if (!_customer.IsValid)
            //    throw new InvalidOperationException(Strings.CustomerViewModel_Exception_CannotSave);

            //if (this.IsNewCustomer)
            //    _customerRepository.AddCustomer(_customer);
            
            //base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this customer was created by the user and it has not yet
        /// been saved to the customer repository.
        /// </summary>
        //bool IsNewCustomer
        //{
        //    get { return !_customerRepository.ContainsCustomer(_customer); }
        //}

        /// <summary>
        /// Returns true if the customer is valid and can be saved.
        /// </summary>
        bool CanSave
        {
           get {return true;} // get { return String.IsNullOrEmpty(this.ValidateCustomerType()) && _customer.IsValid; }
        }

        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_item as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;

                //if (propertyName == "CustomerType")
                //{
                //    // The IsCompany property of the Customer class 
                //    // is Boolean, so it has no concept of being in
                //    // an "unselected" state.  The ItemViewModel
                //    // class handles this mapping and validation.
                //    error = this.ValidateCustomerType();
                //}
                //else
                //{
                //    error = (_item as IDataErrorInfo)[propertyName];
                //}

                //// Dirty the commands registered with CommandManager,
                //// such as our Save command, so that they are queried
                //// to see if they can execute now.
                //CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }

        //string ValidateCustomerType()
        //{
        //    if (this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Company ||
        //       this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Person)
        //        return null;

        //    return Strings.CustomerViewModel_Error_MissingCustomerType;
        //}

        #endregion // IDataErrorInfo Members
    }
}