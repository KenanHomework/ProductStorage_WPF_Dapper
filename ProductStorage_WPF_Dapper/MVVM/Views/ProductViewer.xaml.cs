using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using ProductStorage_WPF_Dapper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.AxHost;

namespace ProductStorage_WPF_Dapper.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ProductViewer.xaml
    /// </summary>
    public partial class ProductViewer : Window
    {
        public ProductViewer()
        {
            InitializeComponent();
            App.Container.GetInstance<ProductViewerVM>().Window = this;
            DataContext = App.Container.GetInstance<ProductViewerVM>();
            ProductListView.ItemsSource = App.Products;
        }

        public bool State { get; set; } = false;

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {


            if (sender is Button btn)
            {
                switch (btn.Content.ToString())
                {
                    case "_":
                        this.WindowState = WindowState.Minimized;
                        break;
                    case "X":
                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!State && ProductListView.SelectedIndex >= 0)
            {

                EditProduct editProduct = new((Product)ProductListView.SelectedItem);
                App.Container.GetInstance<EditProductVM>().Product = (Product)ProductListView.SelectedItem;
                editProduct.CountryComboBox.ItemsSource = App.Countries;
                editProduct.CountryComboBox.SelectedItem = (Product)ProductListView.SelectedItem;
                editProduct.ShowDialog();
                App.Container.GetInstance<ProductViewerVM>().InitializeProductListView();
            }
            else if (ProductListView.SelectionMode == SelectionMode.Multiple)
                ProductListView.SelectedItems.Add(ProductListView.SelectedItem);
        }

        public void ReadyForDeleting(bool state)
        {
            State = state;
            MainFunctionalityGrid.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            DeleteFunctionalityStackPanel.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
            if (ProductListView.SelectionMode == SelectionMode.Extended) ProductListView.SelectedItems.Clear();
            else { ProductListView.SelectedIndex = -1; }
            ProductListView.SelectionMode = state ? SelectionMode.Extended : SelectionMode.Single;
        }

    }
}
