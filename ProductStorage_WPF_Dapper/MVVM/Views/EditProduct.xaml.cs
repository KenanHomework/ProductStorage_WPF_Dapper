using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using ProductStorage_WPF_Dapper.MVVM.Models.WebModels;
using ProductStorage_WPF_Dapper.MVVM.ViewModels;
using ProductStorage_WPF_Dapper.Services;
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

namespace ProductStorage_WPF_Dapper.MVVM.Views
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public EditProduct(Product product )
        {
            InitializeComponent();
            App.Container.GetInstance<EditProductVM>().UrlTempPP = CloudinaryService.GetSource("tempdriverpp", App.TempCloudinaryFolderPath);
            App.Container.GetInstance<EditProductVM>().Window = this;
            App.Container.GetInstance<EditProductVM>().Product = product;
            Price.Text = product.Price.ToString();
            CountryComboBox.SelectedIndex = App.Countries.FindIndex(c => c.Name == product.OriginCountry);
            ProductImage.ImageSource = BitmapService.GetBitmapImageFromUrl(product.ImageSource);

            DataContext = App.Container.GetInstance<EditProductVM>();
        }
        public bool Result { get; set; } = false;

        public string OriginCountry { get; set; }

        #region ControlsCheck

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
                        => RegxService.CheckControl(ref Name, 4, Color.FromRgb(237, 236, 239), "(-?([A-Z].\\s)?([A-Z][a-z]+)\\s?)+([A-Z]'([A-Z][a-z]+))?");

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
                        => RegxService.CheckPrice(ref Price, Color.FromRgb(237, 236, 239));

        private void Count_TextChanged(object sender, TextChangedEventArgs e)
                        => RegxService.CheckPrice(ref Count, Color.FromRgb(237, 236, 239));

        #endregion

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
                        this.Close();
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Country country = CountryComboBox.SelectedItem as Country;

            CountryName.Content = country.Name;
            App.Container.GetInstance<EditProductVM>().Product.OriginCountry = country.Name;

            Flag.ImageSource = BitmapService.GetBitmapImageFromUrl(CountryService.GetFlagByNameOrCode(country));
        }

    }
}
