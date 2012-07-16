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

    class AllItemsViewModel : WorkspaceViewModel
    {  
        LittleTravellerDataContext context;
       
       public  AllItemsViewModel()
        {
             context = new LittleTravellerDataContext();
             this.DisplayName = "View All Items";
             FillItemsGrid();

        }

       private void FillItemsGrid()
       {
           List<AllItemsGridView> ItemsLst = context.AllItemsGridViews.ToList<AllItemsGridView>();
           ItemsGrid = new ReactiveCollection<AllItemsGridView>();
           foreach (AllItemsGridView igv in ItemsLst)
           {
               ItemsGrid.Add(igv);
           }
           DeleteItemCommand = new ReactiveCommand();
           DeleteItemCommand.OfType<AllItemsGridView>().Subscribe(item => DeleteItem(item));
       }
        
       private ReactiveCollection<AllItemsGridView> _itemsGrid;

       // the grid is bound to this
       public ReactiveCollection<AllItemsGridView> ItemsGrid
       {
           get
           {
               return _itemsGrid;
           }
           set
           {
               this.RaiseAndSetIfChanged(vm => vm.ItemsGrid, ref _itemsGrid, value);
           }
       }
    

       public ReactiveCommand DeleteItemCommand { get; private set; }
       public void DeleteItem(AllItemsGridView itemToDelete)
       {
         context.Items.DeleteOnSubmit(context.Items.FirstOrDefault(it => it.Sku.Equals(itemToDelete.Sku)));
         context.SubmitChanges();
         FillItemsGrid();
       //  this.RaisePropertyChanged(vm => vm.ItemsGrid);
       }
       
    }
}
