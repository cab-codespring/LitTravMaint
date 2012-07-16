using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitTravData.Model;
using System.Data;
using System.Windows.Controls;
using ReactiveUI.Xaml;
using ReactiveUI;
using System.Reactive.Linq;

namespace LitTravProj.ViewModel
{

    class AllCustomersViewModel : WorkspaceViewModel
    {
        LittleTravellerDataContext context;

       
    public AllCustomersViewModel()
        {
             context = new LittleTravellerDataContext();
             this.DisplayName = "View All Customers";
             FillItemsGrid();

        }

       private void FillItemsGrid()
       {
           List<Customer> ItemsLst = context.Customers.ToList<Customer>();
           CustsGrid = new ReactiveCollection<Customer>();
           foreach (Customer cgv in ItemsLst)
           {
               CustsGrid.Add(cgv);
           }
           DeleteCustomerCommand = new ReactiveCommand();
           DeleteCustomerCommand.OfType<Customer>().Subscribe(customer => DeleteCustomer(customer));
       }

       private ReactiveCollection<Customer> _custsGrid;

       // the grid is bound to this
       public ReactiveCollection<Customer> CustsGrid
       {
           get
           {
               return _custsGrid;
           }
           set
           {
               this.RaiseAndSetIfChanged(vm => vm.CustsGrid, ref _custsGrid, value);
           }
       }
    

       public ReactiveCommand DeleteCustomerCommand { get; private set; }
       public void DeleteCustomer(Customer customerToDelete)
       {
         context.Customers.DeleteOnSubmit(context.Customers.FirstOrDefault(it => it.CustomerNum.Equals(customerToDelete.CustomerNum)));
         context.SubmitChanges();
         FillItemsGrid();
       //  this.RaisePropertyChanged(vm => vm.ItemsGrid);
       }
       
    }
}

