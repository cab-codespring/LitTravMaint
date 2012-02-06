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

            this.DisplayName = "Add or Edit Item";

            context = new LittleTravellerDataContext();
            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);

            SeasonOptions = context.Seasons.ToList();
            ColorOptions1 = context.Colors.ToList();
            ColorOptions2 = context.Colors.ToList();
            ColorOptions3 = context.Colors.ToList();
            SizeTypeOptions = context.SizeTypes.ToList();
            SizeOptions = context.Sizes.ToList();
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
                SelectedColor1 = ColorOptions1.FirstOrDefault(ssn => ssn.ColorCode == _item.ColorID);
                SelectedColor2 = ColorOptions2.FirstOrDefault(ssn => ssn.ColorCode == _item.Color2ID);
                SelectedColor3 = ColorOptions3.FirstOrDefault(ssn => ssn.ColorCode == _item.Color3ID);
                SelectedSizeTypeID = SizeTypeOptions.FirstOrDefault(ssn => ssn.SizeTypeName == _item.SizeType);
                SelectedSize = SizeOptions.FirstOrDefault(ssn => ssn.SizeVal == _item.Size);
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
        /// Returns a list of Colors used to populate the Color selector.
        /// </summary>
        public IEnumerable<Color> ColorOptions1 { get; private set; }
        public IEnumerable<Color> ColorOptions2 { get; private set; }
        public IEnumerable<Color> ColorOptions3 { get; private set; }

        private Color _selectedColor1 = new Color();

        public Color SelectedColor1 
        {
            get { return _selectedColor1; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm._selectedColor1, ref _selectedColor1, value);
            }
        }


        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
        private Color _selectedColor2 = new Color();

        public Color SelectedColor2 
        {
            get { return _selectedColor2; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm._selectedColor2, ref _selectedColor2, value);
            }
        }
        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
        private Color _selectedColor3 = new Color();

        public Color SelectedColor3
        {
            get { return _selectedColor3; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm._selectedColor3, ref _selectedColor3, value);
            }
        }
        /// <summary>
        /// Returns a list of strings used to populate the Color selector.
        /// </summary>
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
                // this.RaisePropertyChanged(vm => vm.SizeOptions);
            }
        }

        /// <summary>
        /// Returns a list of Sizes used to populate the Size selector.
        /// </summary>
        public IEnumerable<Size> SizeOptions
        {
            get
            {
                if (SelectedSizeTypeID == null)
                    return context.Sizes;
                return from sz in context.Sizes where sz.SizeTypeName.CompareTo(SelectedSizeTypeID.SizeTypeName) == 0 select sz;
            }
            set
            {

            }
        }


      

        /// <summary>
        /// Size must be limited to this size Type
        /// </summary>
        private Size _selectedSize = new Size();
        public Size SelectedSize
        {

            get { return _selectedSize; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedSize, ref _selectedSize, value);
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