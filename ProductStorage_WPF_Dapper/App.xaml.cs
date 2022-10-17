using CloudinaryDotNet;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Contexts;
using ProductStorage_WPF_Dapper.MVVM.Models.WebModels;
using ProductStorage_WPF_Dapper.MVVM.ViewModels;
using ProductStorage_WPF_Dapper.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ProductStorage_WPF_Dapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Members

        public static string ConnStr { get; set; }
        public static string SuccesSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661935108/WolfTaxi/success-sound-effect.mp3";
        public static string ErrorSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661936264/WolfTaxi/error-sound.mp3";
        public static string NotificationSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661940169/WolfTaxi/notification-sound.mp3";
        public static Cloudinary cloudinary = new(new Account("kysbv", "338497835255375", "iz_nsuDxVjxd-zU2xeDncLQDt64"));
        public static string ProductCloudinaryFolderPath = "ProductStorage/ProductsPhotos";
        public static string TempCloudinaryFolderPath = "ProductStorage/TempPhotos";
        public static List<Country> Countries { get; set; } = new();
        public static List<Product> Products { get; set; } = new();
        public static Container Container = new();

        #endregion

        #region Methods

        void ReadConnectionString()
        {
            using (JsonDocument json = JsonDocument.Parse(File.ReadAllText("appconfig.json")))
            {
                JsonElement element = json.RootElement.GetProperty("SqlConnectionString");
                ConnStr = element.ToString();
            }
        }

        void CreateEntityContext()
        {
            using (EntityContext context = new()) { }
        }

        void InitiazlizeMembers()
        {
            Countries = CountryService.ConvertDictToList(JsonService.DowloadSerializedData<Dictionary<string, string>>(@"https://flagcdn.com/en/codes.json"));
            Products = ProductService.GetAllProducts();
        }

        void Register()
        {
            Container.RegisterSingleton<ProductViewerVM>();
            Container.RegisterSingleton<AddProductVM>();
            Container.RegisterSingleton<EditProductVM>();

            Container.Verify();
        }

        void AddDemoProducts()
        {
            if (Products.Count > 0)
                return;

            int index = -1;

            try
            {
                index  = ProductService.Add(new Product()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description="Demo Product",
                    Count = 1,
                    ImageSource = "",
                    Price = 123.5M
                });

                ProductService.Remove(index);
            }
            catch (Exception) { }

            if (index != 1)
                return;

            Product chorek = new()
            {
                Name = "Çörək Təndir",
                Description = "Bol olsa çörək basılmaz VƏTƏN!!",
                OriginCountry = "Azerbaijan",
                Price = 1.0M,
                Count = 100,
                ImageSource = @"https://i.ytimg.com/vi/vH65RhHMMmQ/maxresdefault.jpg"
            };

            Product un = new()
            {
                Name = "Un ƏLa Növ 1kq",
                Description = "Əle növ un hər növ istifadəyə uyğundur.",
                OriginCountry = "Azerbaijan",
                Price = 3.5M,
                Count = 100,
                ImageSource = @"https://qaymaq.az/uploads/5f6aeff9a25cff50bb0a6c395de0aaa924abf8f42abea.png"
            };

            Product dushes = new()
            {
                Name = "Duşes 1l",
                Description = "Armudlu Duşes 1l",
                OriginCountry = "Azerbaijan",
                Price = 1.4M,
                Count = 100,
                ImageSource = @"https://th.bing.com/th/id/R.8e8bd6a74030f0574416860aea6e75fd?rik=9pWhkyCYg8xtFA&pid=ImgRaw&r=0"
            };

            Products.Add(chorek);
            Products.Add(un);
            Products.Add(dushes);

            try
            {
                ProductService.Add(chorek);
            }
            catch (Exception) { }
            try
            {
                ProductService.Add(un);
            }
            catch (Exception) { }
            try
            {
                ProductService.Add(dushes);
            }
            catch (Exception) { }

        }

        #endregion

        public App()
        {
            ReadConnectionString();
            CreateEntityContext();
            InitiazlizeMembers();
            Register();
            AddDemoProducts();

        }
    }
}
