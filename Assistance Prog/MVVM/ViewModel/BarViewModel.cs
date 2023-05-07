using Assistance_Prog.Core;
using Assistance_Prog.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Schema;

namespace Assistance_Prog.MVVM.ViewModel
{
    class BarViewModel : ObservableObject
    {
        MainViewModel MainVM;

        private IList<BarProduct> bar = new List<BarProduct>();
        private ObservableCollection<BarProduct> chosenProducts = new ObservableCollection<BarProduct>();

        static private readonly string barFolder = "/Data/bar.txt";

        private void loadBar()
        {
            string[] lines;

            if (File.Exists(Directory.GetCurrentDirectory() + barFolder))
            {
                lines = File.ReadAllLines(Directory.GetCurrentDirectory() + barFolder);
            }
            else
            {
                throw new DirectoryNotFoundException("Error loading bar");
            }

            foreach (string line in lines)
            {
                string[] data = line.Split(";");
                BarProduct prod = new BarProduct(data[0], int.Parse(data[1]));
                bar.Add(prod);
            }
        }

        public IList<BarProduct> Bar
        {
            get { return bar; }
            set { bar = value; }
        }
        public ObservableCollection<BarProduct> ChosenProducts
        {
            get { return chosenProducts; }
            set { chosenProducts = value; }
        }

        private BarProduct selectedBar;

        public BarProduct SelectedBar
        {
            get { return selectedBar; }
            set { selectedBar = value; }
        }

        private BarProduct selectedProduct;

        public BarProduct SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }
        public ICommand AddProduct { get; set; }
        private void AddProductClick()
        {
            if (selectedBar.Name != String.Empty)
            {
                chosenProducts.Add(selectedBar);
                SelectedBar = null;
                OnPropertyChanged(nameof(SelectedBar));
            }
        }

        public ICommand DeleteProduct { get; set; }
        private void DeleteProductClick()
        {
            if (selectedProduct.Name != String.Empty)
            {
                chosenProducts.Remove(selectedProduct);
                SelectedProduct = null;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private string room;

        public string Room
        {
            get { return room; }
            set { room = value;
                OnPropertyChanged(nameof(Room));
            }
        }

        public ICommand Done { get; set; }
        private void DoneClick()
        {
            if (MainVM.user.getPermission() == Permission.Bartender || MainVM.user.getPermission() == Permission.Manager)
            {
                if (Room == null)
                {
                    MessageBox.Show("Enter room number");
                    return;
                }

                int number;

                if (!int.TryParse(Room, out number))
                {
                    MessageBox.Show("Mistake in room number");
                    return;
                }

                if (chosenProducts.Count == 0)
                {
                    MessageBox.Show("You haven't chosen any product");
                    return;
                }

                string context = CollectData();
                string date = DateTime.Today.ToString()[..^8];
                string fileName = Directory.GetCurrentDirectory() + "/OutputData/Bartender/" + date + "_room#" + Room + ".txt";
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] text = new UTF8Encoding(true).GetBytes(context);
                    fs.Write(text, 0, text.Length);
                }

                Room = null;
                OnPropertyChanged(nameof(Room));
            }
            else
            {
                MessageBox.Show("Not enough permissions to do it");
            }
        }

        private string CollectData()
        {
            string res = "";
            res += DateTime.Now.ToString() + "\n";
            res += "Room #" + Room + "\n";
            res += "Bartender id: " + MainVM.user.getId().ToString() + "\n";
            res += "Ordered products:\n\n";
            int price = 0;
            foreach (BarProduct prod in chosenProducts)
            {
                res += prod.Name + "  : " + prod.Price + "\n";
                price += prod.Price;
            }
            res += "\nTotal price: " + price.ToString();

            return res;
        }


        public BarViewModel(MainViewModel MainVM) 
        {
            this.MainVM = MainVM;

            try
            {
                loadBar();
            } catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }

            AddProduct = new RelayCommand(o => AddProductClick());
            DeleteProduct = new RelayCommand(o => DeleteProductClick());
            Done = new RelayCommand(o => DoneClick());
        }
    }
}
