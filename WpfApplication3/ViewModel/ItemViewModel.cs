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
    /// <summary>
    /// A UI-friendly wrapper for an Item object.
    /// </summary>
    public class ItemViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private LittleTravellerDataContext context;
        private string _sku;
        private Item _item;



        public ItemViewModel()
        {

            this.DisplayName = "Add New Item";

            context = new LittleTravellerDataContext();
            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);
            SeasonOptions = context.Seasons.ToList();
        }

        public ItemViewModel(Item itemIn)
        {
            this._item = itemIn;
        }

        public string SKU
        {
            get { return _sku; }

            set
            {
                _sku = value;
                if (_sku.Length == 0)
                    return;
                Item existingItem = null;
                 // first or default returns null if none
                 existingItem = context.Items.FirstOrDefault(it => it.Sku.Equals(_sku));
                 if (existingItem == null)
                 {
                     //item doesn;t exist              
                     _item = new Item();
                     _item.Sku = _sku;
                     return; // new sku
                 }
                _item = existingItem;

               
                SelectedSeason = SeasonOptions.FirstOrDefault(ssn => ssn.SeasonCode == _item.SeasonID);
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

        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
        public IEnumerable<Color> ColorOptions
        {
            get
            {
                return context.Colors;
            }
        }

        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
        public IEnumerable<SizeType> SizeTypeOptions
        {
            get
            {
                return context.SizeTypes;
            }
        }

        /// <summary>
        /// Size must be limited to this size Type
        /// </summary>
        //public SizeType SelectedSizeTypeID
        //{
        //    get;
        //    set;
        //}

        SizeType _selectedSizeTypeID;

        /// <summary>
        /// Size must be limited to this size Type
        /// </summary>

        public SizeType SelectedSizeTypeID
        {

            get { return _selectedSizeTypeID; }
            set
            {
                _selectedSizeTypeID = value;
                //this.RaiseAndSetIfChanged(vm => vm.SelectedSizeTypeID, ref _selectedSizeTypeID, value);
                this.RaisePropertyChanged(vm => vm.SizeOptions);
            }
        }

        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
        public IEnumerable<Size> SizeOptions
        {
            get
            {
                if (SelectedSizeTypeID == null)
                    return context.Sizes;
                return from sz in context.Sizes where sz.SizeTypeName == SelectedSizeTypeID.SizeTypeName select sz;
            }
        }

        /// <summary>
        /// Returns a list of strings used to populate the DesignID selector.
        /// </summary>
        public IEnumerable<Design> DesignOptions
        {
            get
            {
                return context.Designs;
            }
        }





        private bool ValidateFields()
        {
            return false;
        }




        public ReactiveCommand SaveCommand { get; private set; }


        ///// <summary>
        ///// Gets/sets whether this customer is selected in the UI.
        ///// </summary>
        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        if (value == _isSelected)
        //            return;

        //        _isSelected = value;

        //        base.OnPropertyChanged("IsSelected");
        //    }
        //}

        ///// <summary>
        ///// Returns a command that saves the customer.
        ///// </summary>
        //public ICommand SaveCommand
        //{
        //    get
        //    {
        //        if (_saveCommand == null)
        //        {
        //            _saveCommand = new RelayCommand(
        //                param => this.Save()
        //                //  param => this.CanSave
        //                );
        //        }
        //        return _saveCommand;
        //    }
        //}

        // Presentation Properties

        // Public Methods

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        //public void Save()
        //{
        //    //if (!_customer.IsValid)
        //    //    throw new InvalidOperationException(Strings.CustomerViewModel_Exception_CannotSave);

        //    //if (this.IsNewCustomer)
        //    //    _customerRepository.AddCustomer(_customer);

        //    //base.OnPropertyChanged("DisplayName");
        //}

        // Public Methods

        // Private Helpers

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
        //bool CanSave
        //{
        //    get { return true; } // get { return String.IsNullOrEmpty(this.ValidateCustomerType()) && _customer.IsValid; }
        //}



        //#region IDataErrorInfo Members

        //string IDataErrorInfo.Error
        //{
        //    get { return (_item as IDataErrorInfo).Error; }
        //}

        //string IDataErrorInfo.this[string propertyName]
        //{
        //    get
        //    {
        //        string error = null;

        //        //if (propertyName == "CustomerType")
        //        //{
        //        //    // The IsCompany property of the Customer class 
        //        //    // is Boolean, so it has no concept of being in
        //        //    // an "unselected" state.  The ItemViewModel
        //        //    // class handles this mapping and validation.
        //        //    error = this.ValidateCustomerType();
        //        //}
        //        //else
        //        //{
        //        //    error = (_item as IDataErrorInfo)[propertyName];
        //        //}

        //        //// Dirty the commands registered with CommandManager,
        //        //// such as our Save command, so that they are queried
        //        //// to see if they can execute now.
        //        //CommandManager.InvalidateRequerySuggested();

        //        return error;
        //    }
        //}

        ////string ValidateCustomerType()
        ////{
        ////    if (this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Company ||
        ////       this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Person)
        ////        return null;

        ////    return Strings.CustomerViewModel_Error_MissingCustomerType;
        ////}

        //#endregion // IDataErrorInfo Members
    }
}