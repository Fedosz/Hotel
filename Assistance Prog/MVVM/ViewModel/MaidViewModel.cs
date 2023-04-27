using Assistance_Prog.Core;
using Assistance_Prog.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Assistance_Prog.MVVM.ViewModel
{
    class MaidViewModel : ObservableObject
    {
        public MainViewModel MainVM;

        private IList<string> rooms = new List<string>();
        private IList<string> products = new List<string>();
        private ObservableCollection<string> chosenProducts = new ObservableCollection<string>();

        static private readonly string roomsFolder = "/Data/rooms.txt";
        static private readonly string barFolder = "/Data/mini-bar.txt";
        

        private void loadRooms()
        {
            string[] lines;

            if (File.Exists(Directory.GetCurrentDirectory() + roomsFolder))
            {
                lines = File.ReadAllLines(Directory.GetCurrentDirectory() + roomsFolder);
            }
            else
            {
                throw new DirectoryNotFoundException("Error loading rooms");
            }

            foreach (string line in lines)
            {
                string[] data = line.Split(";");
                int isBooked = int.Parse(data[1]);
                if (isBooked == 1)
                {
                    rooms.Add(data[0]);
                }
            }
        }
        private void loadProducts()
        {
            string[] lines;

            if (File.Exists(Directory.GetCurrentDirectory() + barFolder))
            {
                lines = File.ReadAllLines(Directory.GetCurrentDirectory() + barFolder);
            }
            else
            {
                throw new DirectoryNotFoundException("Error loading products");
            }

            foreach (string line in lines)
            {
                products.Add(line);
            }
        }


        public IList<string> Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }
        public IList<string> Products
        {
            get { return products; }
            set { products = value; }
        }
        public ObservableCollection<string> ChosenProducts
        {
            get { return chosenProducts; }
            set { chosenProducts = value; }
        }



        private string selectedRoom;
        public string SelectedRoom
        {
            get { return selectedRoom; }
            set { selectedRoom = value;
            }
        }


        private string selectedProduct;
        public string SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }


        private string selectedChosenProduct;
        public string SelectedChosenProduct
        {
            get { return selectedChosenProduct; }
            set { selectedChosenProduct = value; }
        }


        public ICommand AddProduct { get; set; }
        private void AddProductClick()
        {
            if (selectedProduct != String.Empty)
            {
                chosenProducts.Add(selectedProduct);
                SelectedProduct = null;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public ICommand DeleteProduct { get; set; }
        private void DeleteProductClick()
        {
            if (selectedChosenProduct != String.Empty)
            {
                chosenProducts.Remove(selectedChosenProduct);
                SelectedChosenProduct = null;
                OnPropertyChanged(nameof(SelectedChosenProduct));
            }
        }

        public ICommand Done { get; set; }
        private void DoneClick()
        {
            if (MainVM.user.getPermission() == Permission.HouseMaid || MainVM.user.getPermission() == Permission.Manager)
            {
                if (selectedRoom == null)
                {
                    MessageBox.Show("Select room");
                    return;
                }
                string context = CollectData();
                string date = DateTime.Today.ToString()[..^8];
                string fileName = Directory.GetCurrentDirectory() + "/OutputData/Maid/" + date + "_room#" + selectedRoom + ".txt";
                using (FileStream fs = File.Create(fileName))
                { 
                    Byte[] text = new UTF8Encoding(true).GetBytes(context);
                    fs.Write(text, 0, text.Length);
                }
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
            res += "Room #" + selectedRoom + "\n";
            res += "Maid id: " + MainVM.user.getId().ToString() + "\n";
            res += "Used mini-bar:\n";
            if (chosenProducts.Count == 0)
            {
                res += "none";
            } else {
                foreach (string a in chosenProducts)
                {
                    res += a + "\n";
                }
            }
            return res;
        }


        public MaidViewModel(MainViewModel MainVM) 
        {
            this.MainVM = MainVM;
            try
            {
                loadRooms();
            } catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                loadProducts();
            } catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }

            AddProduct = new RelayCommand(o => AddProductClick());
            DeleteProduct = new RelayCommand(o => DeleteProductClick());
            Done = new RelayCommand(o =>  DoneClick());
        }
    }
}
