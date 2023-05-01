using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Assistance_Prog.MVVM.Model
{
    internal class BarProduct
    {
        private string name { get; set; }
        private int price { get; set; }

        public BarProduct(string name, int price) 
        {
            this.name = name;
            this.price = price;
        }
        public string Name { get { return name; } }
        public int Price { get { return price; } }
        
    }
}
