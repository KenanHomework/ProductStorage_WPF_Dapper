using ProductStorage_WPF_Dapper.MVVM.Models.WebModels;
using ProductStorage_WPF_Dapper.MVVM.ViewModels;
using ProductStorage_WPF_Dapper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProductStorage_WPF_Dapper.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            App.Container.GetInstance<AddProductVM>().Product = new() { ImageSource = @"https://res.cloudinary.com/kysbv/image/upload/v1665962646/General/product_icon.png" };
            App.Container.GetInstance<AddProductVM>().UrlTempPP = CloudinaryService.GetSource("tempdriverpp", App.TempCloudinaryFolderPath);
            App.Container.GetInstance<AddProductVM>().Window = this;
            ProductImage.ImageSource = BitmapService.GetBitmapImageFromUrl(@"https://res.cloudinary.com/kysbv/image/upload/v1665962646/General/product_icon.png");

            DataContext = App.Container.GetInstance<AddProductVM>();
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
            OriginCountry = country.Name;

            Flag.ImageSource = BitmapService.GetBitmapImageFromUrl(CountryService.GetFlagByNameOrCode(country));
        }


        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
