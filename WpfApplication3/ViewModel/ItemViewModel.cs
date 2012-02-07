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
    public class ItemViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private LittleTravellerDataContext context;
      
        private Item _item;
        private bool _itemExists = false;


        public ItemViewModel()
        {

            this.DisplayName = "Add/Edit Item";

            context = new LittleTravellerDataContext();
            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);
            SaveCommand.Subscribe(_ => SaveItem());

            SeasonOptions = context.Seasons.ToList();
            ColorOptions1 = context.Colors.ToList();
            ColorOptions2 = context.Colors.ToList();
            ColorOptions3 = context.Colors.ToList();
            SizeTypeOptions = context.SizeTypes.ToList();
            SizeOptions = context.Sizes.ToList();
            DesignOptions = context.Designs.ToList();
            StyleTypeOptions = context.StyleTypes.ToList();
        }

        public ItemViewModel(Item itemIn)
        {
            this._item = itemIn;
        }

        private string _sku;
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
                    _itemExists = false;
                    return; // new sku
                }
                _item = existingItem;
                _itemExists = true;

                SelectedSeason = SeasonOptions.FirstOrDefault(ssn => ssn.SeasonCode == _item.SeasonID);
                SelectedColor1 = ColorOptions1.FirstOrDefault(ssn => ssn.ColorCode == _item.ColorID);
                SelectedColor2 = ColorOptions2.FirstOrDefault(ssn => ssn.ColorCode == _item.Color2ID);
                SelectedColor3 = ColorOptions3.FirstOrDefault(ssn => ssn.ColorCode == _item.Color3ID);
                SelectedSizeTypeID = SizeTypeOptions.FirstOrDefault(ssn => ssn.SizeTypeName == _item.SizeType);
                SelectedSize = SizeOptions.FirstOrDefault(ssn => ssn.SizeVal == _item.Size);
                SelectedStyleType = StyleTypeOptions.FirstOrDefault(ssn => ssn.ID == _item.StyleTypeID);
                SelectedDesign = DesignOptions.FirstOrDefault(ssn => ssn.ID == _item.DesignID);
                Price = _item.Price.ToString();
                
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
                this.RaiseAndSetIfChanged(vm => vm.SelectedColor1, ref _selectedColor1, value);
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
                this.RaiseAndSetIfChanged(vm => vm.SelectedColor2, ref _selectedColor2, value);
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
                this.RaiseAndSetIfChanged(vm => vm.SelectedColor3, ref _selectedColor3, value);
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
                SizeOptions = (from sz in context.Sizes where sz.SizeTypeName.CompareTo(SelectedSizeTypeID.SizeTypeName) == 0 select sz).ToList();
            }
        }

        /// <summary>
        /// Returns a list of Sizes used to populate the Size selector.
        /// </summary>
        private IEnumerable<Size> _sizeOptions;
        public IEnumerable<Size> SizeOptions
        {
            get { return _sizeOptions; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SizeOptions, ref _sizeOptions, value);
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
        /// Returns a list of Styles used to populate the styletype  selector.
        /// </summary>
        public IEnumerable<StyleType> StyleTypeOptions { get; private set; }

        private StyleType _selectedStyleType = new StyleType();

        public StyleType SelectedStyleType
        {
            get { return _selectedStyleType; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedStyleType, ref _selectedStyleType, value);
            }
        }

        /// <summary>
        /// Returns a list of strings used to populate the DesignID selector.
        /// </summary>
        public IEnumerable<Design> DesignOptions { get; private set; }

        private Design _selectedDesign = new Design();

        public Design SelectedDesign
        {
            get { return _selectedDesign; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedDesign, ref _selectedDesign, value);
            }
        }

        public string Price { get;  set; }

        decimal _validatedPrice = 0;
        private bool ValidateFields()
        {
            if (SKU == null || SKU.Length == 0)
                return false;
            if (Price == null)
                return false;
            try
            {
                _validatedPrice = Decimal.Parse(Price, NumberStyles.Currency);
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        public ReactiveCommand SaveCommand { get; private set; }

        private void SaveItem()
        {

            _item.SeasonID = SelectedSeason.SeasonCode;
            _item.ColorID = SelectedColor1.ColorCode;
            _item.Color2ID = SelectedColor2.ColorCode;
            _item.Color3ID = SelectedColor3.ColorCode;
            _item.SizeType = SelectedSizeTypeID.SizeTypeName;
            _item.Size = SelectedSize.SizeVal;
            _item.StyleTypeID = _selectedStyleType.ID;
            _item.DesignID = SelectedDesign.ID;
            _item.Price = _validatedPrice;

            if (!_itemExists)
                context.Items.InsertOnSubmit(_item);
            context.SubmitChanges();
     
            this.RaiseAndSetIfChanged(vm => vm.SKU, ref _sku, "");
        }


       
    }
}