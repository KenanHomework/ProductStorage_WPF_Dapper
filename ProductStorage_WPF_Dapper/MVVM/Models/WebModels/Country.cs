using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorage_WPF_Dapper.MVVM.Models.WebModels
{
    public class Country
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public override string ToString() => Name;

    }
}
