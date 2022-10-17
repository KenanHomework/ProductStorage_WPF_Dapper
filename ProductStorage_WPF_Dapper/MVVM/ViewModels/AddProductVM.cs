using CloudinaryDotNet.Actions;
using Microsoft.Win32;
using ProductStorage_WPF_Dapper.Commands;
using ProductStorage_WPF_Dapper.Enums;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using ProductStorage_WPF_Dapper.MVVM.Views;
using ProductStorage_WPF_Dapper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProductStorage_WPF_Dapper.MVVM.ViewModels
{
    public class AddProductVM
    {

        #region Members

        public Product Product { get; set; }

        public AddProduct Window { get; set; }

        public bool ProcessResult { get; set; } = false;

        public string UrlTempPP { get; set; } = string.Empty;
        bool dialogResult = false;
        OpenFileDialog ofd;


        #endregion

        #region Command

        public RelayCommand BrowseImage { get; set; }

        public RelayCommand Add { get; set; }

        #endregion

        #region Methods

        public void BrowseRun(object param)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "Image File (* png)| *.png";
            dialogResult = (bool)ofd.ShowDialog();
            CloudinaryService.DestroyImage("tempdriverpp", App.TempCloudinaryFolderPath);
            UrlTempPP = CloudinaryService.UploadImage(ofd.FileName, "tempdriverpp", App.TempCloudinaryFolderPath);
            Window.ProductImage.ImageSource = null;
            Window.ProductImage.ImageSource = BitmapService.GetBitmapImageFromUrl(UrlTempPP);
            SoundService.Succes();
        }

        public void AddRun(object param)
        {
            if (dialogResult)
                CloudinaryService.DestroyImage("tempdriverpp", App.TempCloudinaryFolderPath);
            string imageSource = dialogResult ? CloudinaryService.UploadImage(ofd.FileName, Product.Name, App.ProductCloudinaryFolderPath)
                                              : @"https://res.cloudinary.com/kysbv/image/upload/v1665962646/General/product_icon.png";

            Product temp = new()
            {
                Name = Product.Name,
                Description = Window.Description.Text,
                ImageSource = imageSource,
                Count = Product.Count,
                Price = Convert.ToDecimal(Window.Price.Text),
                OriginCountry = Window.OriginCountry
            };

            int res = -1;

            try
            {
                res = ProductService.Add(temp);
            }
            catch (Exception)
            {
                SoundService.Error();
                CMessageBox.Show("This Product Already Added !", CMessageTitle.Error, CMessageButton.Ok, CMessageButton.None);
                return;
            }

            if (res < 0)
                return;

            Product = new();
            UrlTempPP = null;
            dialogResult = false;
            ProcessResult = true;

            SoundService.Succes();
            Window.Close();
        }

        public bool AddCanRun(object param) => AllInfoCorrect();

        public bool AllInfoCorrect()
                => !string.IsNullOrWhiteSpace(Product.Name) &&
                    !string.IsNullOrWhiteSpace(Product.ImageSource) &&
                    !string.IsNullOrWhiteSpace(Window.OriginCountry) &&
                    !string.IsNullOrWhiteSpace(Window.Price.Text) &&
                    Product.Count >= 0 &&
                    Product.Price >= 0.0M &&
                    Regex.IsMatch(Product.Name, "(-?([A-Z].\\s)?([A-Z][a-z]+)\\s?)+([A-Z]'([A-Z][a-z]+))?") &&
                    RegxService.CheckCount(ref Window.Count, Color.FromRgb(179, 179, 179));


        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public AddProductVM()
        {
            BrowseImage = new(BrowseRun);
            Add = new(AddRun, AddCanRun);
        }


    }
}
