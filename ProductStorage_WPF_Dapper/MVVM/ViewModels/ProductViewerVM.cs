using ProductStorage_WPF_Dapper.Commands;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using ProductStorage_WPF_Dapper.MVVM.Views;
using ProductStorage_WPF_Dapper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Serialization;
using SelectionMode = System.Windows.Controls.SelectionMode;

namespace ProductStorage_WPF_Dapper.MVVM.ViewModels
{
    public class ProductViewerVM
    {

        #region Members

        public ProductViewer Window { get; set; }

        #endregion

        #region Commands

        public RelayCommand Refresh { get; set; }
        public RelayCommand Add { get; set; }

        public RelayCommand Delete { get; set; }

        public RelayCommand Clear { get; set; }

        public RelayCommand DeleteAction { get; set; }

        public RelayCommand CancelAction { get; set; }

        #endregion

        #region Methods

        public void RefreshRun(object param) => InitializeProductListView();

        public void AddRun(object param)
        {
            AddProduct addProduct = new();
            addProduct.CountryComboBox.ItemsSource = App.Countries;   
            addProduct.ShowDialog();
            InitializeProductListView();
        }

        public void DeleteRun(object param)
        {
            Window.ReadyForDeleting(true);
        }

        public bool DeleteCanRun(object param) => App.Products != null && App.Products.Count > 0;

        public void CancelActionRun(object param)
        {
            Window.ReadyForDeleting(false);
        }

        public void DeleteActionRun(object param)
        {
            if (Window.ProductListView.SelectedItems.Count > 0)
            {
                DialogResult result = CMessageBox.Show($"Are you sure you want to delete the products?\nNumber of Products to be deleted: {Window.ProductListView.SelectedItems.Count}", Enums.CMessageTitle.Confirm, Enums.CMessageButton.Yes, Enums.CMessageButton.No);

                if (result != DialogResult.Yes)
                    return;

                foreach (Product item in Window.ProductListView.SelectedItems)
                {
                    try
                    {
                        ProductService.Remove(item);
                    }
                    catch (Exception) { }
                }


                Window.ReadyForDeleting(false);
                InitializeProductListView();

            }
        }

        public void ClearRun(object param)
        {
            try
            {
                ProductService.RemoveRange(App.Products);
            }
            catch (Exception) { }

            App.Products.Clear();
            Window.ProductListView.ItemsSource = null;

        }

        public bool ClearCanRun(object param) => App.Products != null && App.Products.Count > 0;

        public void InitializeProductListView()
        {
            App.Products = ProductService.GetAllProducts();
            Window.ProductListView.ItemsSource = null;
            Window.ProductListView.ItemsSource = App.Products;
            if (Window.ProductListView.SelectionMode == SelectionMode.Extended) Window.ProductListView.SelectedItems.Clear();
            else { Window.ProductListView.SelectedIndex = -1; }
            Window.ProductListView.SelectionMode = SelectionMode.Single;
        }

        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public ProductViewerVM()
        {
            Refresh = new(RefreshRun);
            Add = new(AddRun);
            Clear = new(ClearRun, ClearCanRun);
            Delete = new(DeleteRun, DeleteCanRun);
            CancelAction = new(CancelActionRun);
            DeleteAction = new(DeleteActionRun);
        }

    }
}
