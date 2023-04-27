using Assistance_Prog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Assistance_Prog.MVVM.ViewModel
{
    class MaidViewModel : ObservableObject
    {
        private IList<string> rooms = new List<string>();
        static private readonly string roomsFolder = "/Data/rooms.txt";
        static private readonly string barFolder = "/Data/mini-bar.txt";
        private IList<string> products = new List<string>();
        private IList<string> chosenProducts = new List<string>();
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
        public IList<string> ChosenProducts
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

        public MaidViewModel() 
        {
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


        }
    }
}
