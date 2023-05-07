using Assistance_Prog.Core;
using Assistance_Prog.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Assistance_Prog.MVVM.ViewModel
{
    class AdminViewModel : ObservableObject
    {
        MainViewModel MainVM;

        private ObservableCollection<string> rooms = new ObservableCollection<string>();
        static private readonly string roomsFolder = "/Data/rooms.txt";

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string surName;
        public string SurName
        {
            get { return surName; }
            set
            {
                surName = value;
                OnPropertyChanged(nameof(SurName));
            }
        }

        private string thirdName;
        public string ThirdName
        {
            get { return thirdName; }
            set
            {
                thirdName = value;
                OnPropertyChanged(nameof(ThirdName));
            }
        }

        private string passport;
        public string Passport
        {
            get { return passport; }
            set
            {
                passport = value;
                OnPropertyChanged(nameof(Passport));
            }
        }

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
                if (isBooked == 0)
                {
                    rooms.Add(data[0]);
                }
            }
        }

        public ObservableCollection<string> Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }

        private string selectedRoom;
        public string SelectedRoom
        {
            get { return selectedRoom; }
            set { selectedRoom = value; }
        }

        public ICommand Done { get; set; }
        private void DoneClick()
        {
            if (MainVM.user.getPermission() == Permission.Administrator || MainVM.user.getPermission() == Permission.Manager)
            {
                if (selectedRoom == null || SurName == null || Name == null || Passport == null)
                {
                    MessageBox.Show("Enter full information");
                    return;
                }
                string context = CollectData();
                string date = DateTime.Today.ToString()[..^8];
                string fileName = Directory.GetCurrentDirectory() + "/OutputData/Administrator/" + date + "_room#" + selectedRoom + ".txt";
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] text = new UTF8Encoding(true).GetBytes(context);
                    fs.Write(text, 0, text.Length);
                }
                rooms.Remove(selectedRoom);
                SelectedRoom = null;
                OnPropertyChanged(nameof(SelectedRoom));
                Name = null;
                OnPropertyChanged(nameof(Name));
                SurName = null;
                OnPropertyChanged(nameof(SurName));
                ThirdName = null;
                OnPropertyChanged(nameof(ThirdName));
                Passport = null;
                OnPropertyChanged(nameof(Passport));
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
            res += "Administator id: " + MainVM.user.getId().ToString() + "\n";
            res += "Client information:\n";
            res += "Name = " + Name + "\n";
            res += "Surname = " + SurName + "\n";
            res += "Thirdname = " + ThirdName + "\n";
            res += "Passport number = " + Passport + "\n";

            return res;
        }

        public AdminViewModel(MainViewModel MainVM)
        {
            this.MainVM = MainVM;

            try
            {
                loadRooms();
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }

            Done = new RelayCommand(o => DoneClick());
        }
    }
}
