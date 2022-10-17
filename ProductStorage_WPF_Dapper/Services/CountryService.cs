using ProductStorage_WPF_Dapper.MVVM.Models.WebModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorage_WPF_Dapper.Services
{
    public abstract class CountryService
    {

        public static string GetFlagByNameOrCode(string nameOrCode) => $@"https://flagcdn.com/48x36/{nameOrCode}.png";

        public static string GetFlagByNameOrCode(Country country) => $@"https://flagcdn.com/48x36/{country.Code}.png";

        public static List<Country> ConvertDictToList(Dictionary<string,string> dict)
        {
            List<Country> countries = new();
            countries.AddRange(dict.Select(item => new Country() { Name = item.Value, Code = item.Key }));

            return countries;
        }

    }
}
