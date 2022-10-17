using ProductStorage_WPF_Dapper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes
{
    public class Product
    {
        private int count;
        private decimal price;
        private string originCountry;
        private string imageSource;
        private string description;
        private string name;


        public int Id { get; set; }


        public string Name
        {
            get => name; set { name = value; OnPropertyChanged(); }
        }

        public string? Description
        {
            get => description; set { description = value; OnPropertyChanged(); }
        }

        public string ImageSource
        {
            get => imageSource; set { imageSource = value; OnPropertyChanged(); }
        }

        public string OriginCountry
        {
            get => originCountry; set { originCountry = value; OnPropertyChanged(); }
        }

        public decimal Price
        {
            get => price; set { price = value; OnPropertyChanged(); }
        }

        public int Count
        {
            get => count; set { count = value; OnPropertyChanged(); }
        }

        public string Flag => CountryService.GetFlagByNameOrCode(OriginCountry);

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
