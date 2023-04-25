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
        private IList<int> rooms = new List<int>();
        static private readonly string rootFolder = "/Data/rooms.txt";
        private void loadRooms()
        {
            string[] lines;

            if (File.Exists(Directory.GetCurrentDirectory() + rootFolder))
            {
                lines = File.ReadAllLines(Directory.GetCurrentDirectory() + rootFolder);
            }
            else
            {
                throw new DirectoryNotFoundException("Error loading rooms");
            }

            foreach (string line in lines)
            {
                string[] data = line.Split(";");
                int room = int.Parse(data[0]);
                int isBooked = int.Parse(data[1]);
                if (isBooked == 1)
                {
                    rooms.Add(room);
                }
            }
        }

        public IList<int> Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }

        private int selectedRoom;

        public int SelectedRoom
        {
            get { return selectedRoom; }
            set { selectedRoom = value; }
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

        }
    }
}
