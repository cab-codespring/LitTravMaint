using System;
using System.ComponentModel;
using System.Windows.Input;
using LitTravData.Model;
using System.Text.RegularExpressions;
using LitTravProj.Properties;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Collections;

namespace LitTravProj.ViewModel
{
    /// <summary>
    /// view model for order
    /// </summary>
    public class OrderViewModel : WorkspaceViewModel, INotifyPropertyChanged
    {
        private LittleTravellerDataContext context;

        private Order _order;
        private bool _orderExists = false;
        bool _hasChanges = false;
        public OrderViewModel()
        {

            this.DisplayName = "Add/Edit Order";

            context = new LittleTravellerDataContext();
            var CanSave = this.Changed.Select(_ => ValidateFields()).StartWith(false);
            SaveCommand = new ReactiveCommand(CanSave);
            SaveCommand.Subscribe(_ => SaveOrder());
            NewOrderNumCommand = new ReactiveCommand();
            NewOrderNumCommand.Subscribe(_ => NewOrderNum());
            CloseTabCommand = new ReactiveCommand();
            CloseTabCommand.Subscribe(_ => TabClosing());
            SaveCommand.Subscribe(_ => SaveOrder());
            var CanAddItem = this.Changed.Select(_ => CanAddItemValidate()).StartWith(false);
            AddItemCommand = new ReactiveCommand(CanAddItem);
            AddItemCommand.Subscribe(_ => AddItem());
            DeleteItemCommand = new ReactiveCommand();
            DeleteItemCommand.OfType<ItemOptionsClass>().Subscribe(item => DeleteItem(item));

            CustomerOptions = (from cs in context.Customers select cs.CompanyName).ToList();
            SeasonOptions = context.Seasons.ToList();
            SizeTypeOptions = context.SizeTypes.ToList();
            ItemOptions = new ReactiveCollection<ItemOptionsClass>();
            _orderItems = new ObservableCollection<ItemOptionsClass>();
            AllSeasonsChecked = true;
            AllSizeTypesChecked = true;
          

            FillItemOptions();

        }

        public ReactiveCommand SaveCommand { get; private set; }

        private void SaveOrder()
        {
            if (!_orderExists)
            {
                _order.OrderDate = DateTime.Now;

                foreach (ItemOptionsClass ioc in OrderItems)
                {
                    OrderItem oi = new OrderItem();
                    oi.Sku = ioc.Sku;
                    oi.Quantity = Int16.Parse(ioc.Quantity);
                    oi.OrderNum = _order.OrderNum;
                    context.OrderItems.InsertOnSubmit(oi);
                }

                context.Orders.InsertOnSubmit(_order);
            }
            else
            {
                foreach (ItemOptionsClass ioc in OrderItems)
                { 
                    // add new items
                    OrderItem oi = new OrderItem();
                    oi.Sku = ioc.Sku;
                    oi.Quantity = Int16.Parse(ioc.Quantity);
                    oi.OrderNum = _order.OrderNum;
                    if (!context.OrderItems.Contains(oi))
                        context.OrderItems.InsertOnSubmit(oi);
                }
                
            }
            context.SubmitChanges();

            this.RaiseAndSetIfChanged(vm => vm.OrderNum, ref _orderNum, "");
            _hasChanges = false;
        }

        private bool ValidateFields()
        {
            if (OrderItems == null || OrderItems.Count<ItemOptionsClass>() == 0)
                return false;
            if (SelectedCustomer == null)
                return false;
            // the setter for OrderNum sets the field to "" if it doesn't parse
            if (_orderNum.Length == 0)
                return false;
            return true;
        }


        private string _orderNum = "";
        public string OrderNum
        {
            get { return _orderNum; }

            set
            {
                if (_hasChanges)
                {
                    DataLossWarningDlg warn = new DataLossWarningDlg();
                    warn.ShowDialog();
                    if (warn.DialogResult != System.Windows.Forms.DialogResult.OK)
                        return;

                }
                short ordNumParsed;
                if (!Int16.TryParse(value, out ordNumParsed))
                {
                    _orderNum = "";
                    return;
                }
                if (ordNumParsed == 0)
                {
                    _orderNum = "";
                    return;
                }
                if (value.Length == 0)
                    return;

                Order existingOrder = null;
                // first or default returns null if none
                existingOrder = context.Orders.FirstOrDefault(it => it.OrderNum.Equals(ordNumParsed));
                if (existingOrder == null)
                {  // if there are changes show warning window and process result 
                 //   if(true) // (CanSave
                  //    open DatalossWaningDlg

                    //item doesn;t exist        
                    _order = new Order();
                    _order.OrderNum = ordNumParsed;
                    _orderExists = false;
                    this.RaiseAndSetIfChanged(vm => vm.OrderNum, ref _orderNum, value);
                }
                else
                {
                    _order = existingOrder;
                    _orderNum = _order.OrderNum.ToString();
                    SelectedCustomerNum = _order.CustomerNum;

                    _orderExists = true;
                    this.RaisePropertyChanged(vm => vm.OrderNum);
                    string companyName = context.Customers.FirstOrDefault(it => it.CustomerNum.Equals(_order.CustomerNum)).CompanyName;
                    this.RaiseAndSetIfChanged(vm => vm.SelectedCustomer, ref _selectedCustomer, companyName);
                    // load items from db into orderItems
                    foreach (OrderItem oi in context.OrderItems)
                    {
                        ItemOptionsClass ioc = new ItemOptionsClass();
                        ioc.Sku = oi.Sku;
                        ioc.Quantity = oi.Quantity.ToString();
                        ioc.ItemString = formatItem(context.ItemsGridViews.FirstOrDefault(it => it.Sku.Equals(oi.Sku)));
                        OrderItems.Add(ioc);
                    }
                }

            }
        }

        public ReactiveCommand NewOrderNumCommand { get; private set; }
        public void NewOrderNum()
        {
            string _newOrderNum = ((from order in context.Orders select order.OrderNum).Max() + 1).ToString();
            OrderNum = _newOrderNum;
        }


        /// <summary>
        /// Returns a list of strings used to populate the Customer selector.
        /// </summary>
        public IEnumerable<string> CustomerOptions { get; private set; }

        private string _selectedCustomer;

        public string SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                SelectedCustomerNum = (from cn in context.Customers
                                       where cn.CompanyName == value
                                       select cn.CustomerNum).FirstOrDefault();
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
                AllSeasonsChecked = false;
            }
        }




        public IEnumerable<SizeType> SizeTypeOptions { get; private set; }
        private SizeType _selectedSizeTypeID;
        public SizeType SelectedSizeTypeID
        {

            get { return _selectedSizeTypeID; }
            set
            {

                this.RaiseAndSetIfChanged(vm => vm.SelectedSizeTypeID, ref _selectedSizeTypeID, value);
                AllSizeTypesChecked = false;
            }
        }
        private bool _allSeasonsChecked;
        public bool AllSeasonsChecked
        {

            get { return _allSeasonsChecked; }
            set
            {

                this.RaiseAndSetIfChanged(vm => vm.AllSeasonsChecked, ref _allSeasonsChecked, value);
                FillItemOptions();
            }
        }

        private bool _allSizeTypesChecked;
        public bool AllSizeTypesChecked
        {

            get { return _allSizeTypesChecked; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.AllSizeTypesChecked, ref _allSizeTypesChecked, value);

                FillItemOptions();
            }
        }

        /// <summary>
        ///  litle class for holding items in the items options list
        /// </summary>
        public class ItemOptionsClass
        {
            public ItemOptionsClass() { }
            public ItemOptionsClass(string sku, string itemString)
            {
                this.Sku = sku;
                this.ItemString = itemString;
            }
            public string Sku { get; set; }
            public string ItemString { get; set; }
            public string Quantity { get; set; }
        }

        //  private  System.Linq.IQueryable<LitTravData.Model.ItemsGridView>  _itemOptions;



        private ReactiveCollection<ItemOptionsClass> _itemOptions;
        public ReactiveCollection<ItemOptionsClass> ItemOptions
        {
            get
            {
                return _itemOptions;
            }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.ItemOptions, ref _itemOptions, value);
            }
        }

        private void FillItemOptions()
        {
            ItemOptions.Clear();
            System.Linq.IQueryable<LitTravData.Model.ItemsGridView> tmpTable;
            tmpTable = getItemOptionsData();

            foreach (ItemsGridView itm in tmpTable)
            {
                ItemOptionsClass ioc = new ItemOptionsClass(itm.Sku, formatItem(itm));
                ItemOptions.Add(ioc);
            }
            SelectedItemOption = ItemOptions.First();
        }

        private string formatItem(ItemsGridView itm)
        {
            return itm.Sku + " " + itm.SeasonID + " " +
                    itm.ColorID + " " + itm.Color2ID + " " + itm.Color3ID + " " + itm.SizeType +
                        " " + itm.SizeVal + " " + itm.StyleType + " " + itm.Design + " " + itm.Price;
        }

        private System.Linq.IQueryable<LitTravData.Model.ItemsGridView> getItemOptionsData()
        {
            // the constructor calls this before the checkbozes are checked
            // and it causes the query to be bad as it's looking for null seasons.
            if (SelectedSeason == null && !AllSeasonsChecked ||
                SelectedSizeTypeID == null && !AllSizeTypesChecked)
                return this.context.GetTable<ItemsGridView>();

            if (!AllSeasonsChecked && AllSizeTypesChecked)
                return (from igv in context.ItemsGridViews
                        where igv.SeasonID == SelectedSeason.SeasonCode
                        select igv);
            else if (AllSeasonsChecked && !AllSizeTypesChecked)
                return (from igv in context.ItemsGridViews
                        where igv.SizeType == SelectedSizeTypeID.SizeTypeName
                        select igv);
            else if (!AllSeasonsChecked && !AllSizeTypesChecked)
                return (from igv in context.ItemsGridViews
                        where igv.SizeType == SelectedSizeTypeID.SizeTypeName &&
                            igv.SeasonID == SelectedSeason.SeasonCode
                        select igv);
            return this.context.GetTable<ItemsGridView>();
        }

        private ItemOptionsClass _selectedItemOption = new ItemOptionsClass();
        public ItemOptionsClass SelectedItemOption
        {
            get { return _selectedItemOption; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedItemOption, ref _selectedItemOption, value);
            }
        }

        private ObservableCollection<ItemOptionsClass> _orderItems;
        public ObservableCollection<ItemOptionsClass> OrderItems
        {
            get { return _orderItems; }
            set { _orderItems = value; }
        }

        string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.Quantity, ref _quantity, value);
            }
        }

        public ReactiveCommand AddItemCommand { get; private set; }
        public void AddItem()
        {
            SelectedItemOption.Quantity = Quantity;
            OrderItems.Add(SelectedItemOption);
            _hasChanges = true;
        }

        public ReactiveCommand DeleteItemCommand { get; private set; }
        public void DeleteItem(ItemOptionsClass itemToDelete)
        {
            OrderItems.Remove(itemToDelete);
            OrderItem oi = context.OrderItems.FirstOrDefault(it => it.Sku.Equals( itemToDelete.Sku));
            if (context.OrderItems.Contains(oi))
            {
                context.OrderItems.DeleteOnSubmit(context.OrderItems.FirstOrDefault(it => it.Sku.Equals(itemToDelete.Sku)));
                _hasChanges = true;
            }
        }


        private ItemOptionsClass _selectedOrderItem;
        public ItemOptionsClass SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                this.RaiseAndSetIfChanged(vm => vm.SelectedOrderItem, ref _selectedOrderItem, value);
            }
        }

        public ReactiveCommand CanAddItemCommand { get; private set; }
        public bool CanAddItemValidate()
        {
            short parsedQty;
            if (!Int16.TryParse(Quantity, out parsedQty))
                return false;
            if (parsedQty == 0)
                return false;
            return true;

        }

        
          public ReactiveCommand CloseTabCommand { get; private set; }

          private void TabClosing()
          {
              if (_hasChanges)
              {
                  DataLossWarningCloseDlg warn = new DataLossWarningCloseDlg();
                  warn.ShowDialog();
                  if (warn.DialogResult == System.Windows.Forms.DialogResult.OK)
                      CloseCommand.Execute(this); ;
              }
              
          }


    }
}
