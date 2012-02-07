using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LitTravData.Model;

namespace LitTravProj.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class MainWindowViewModel : WorkspaceViewModel
    {


        ReadOnlyCollection<CommandViewModel> _commands;
        // readonly CustomerRepository _customerRepository;
        ObservableCollection<WorkspaceViewModel> _workspaces;



        public MainWindowViewModel()
        {
            base.DisplayName = "Little Travellers Data Maintenance App";

            //   _customerRepository = new CustomerRepository(customerDataFile);
        }



        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "View All Items",
                    new RelayCommand(param => this.ShowAllItems())),

                new CommandViewModel(
                    "Add or Edit Item",
                    new RelayCommand(param => this.CreateNewItem())),
       
                new CommandViewModel(
                    "View All Customers",
                    new RelayCommand(param => this.ShowAllCustomers()))
            };
        }



        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        void CreateNewItem()
        {
            Item newItem = new Item();
            ItemViewModel workspace = new ItemViewModel();
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);

        }

        void ShowAllItems()
        {
            AllItemsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllItemsViewModel)
                as AllItemsViewModel;

            if (workspace == null)
            {
                workspace = new AllItemsViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void ShowAllCustomers()
        {
            AllCustomersViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllCustomersViewModel)
                as AllCustomersViewModel;

            if (workspace == null)
            {
                workspace = new AllCustomersViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

    }

}
