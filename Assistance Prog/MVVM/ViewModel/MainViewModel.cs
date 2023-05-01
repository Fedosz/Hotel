using Assistance_Prog.Core;
using Assistance_Prog.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Assistance_Prog.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MaidViewCommand { get; set; }
        public RelayCommand BarViewCommand { get; set; }

        public User user;
        public HomeViewModel HomeVM { get; set; }
        public MaidViewModel MaidVM { get; set; }

        public BarViewModel BarVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        private string userName;
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                this.OnPropertyChanged(nameof(UserName));
            }
        }
        private string password;
        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }

        private Visibility imageVisibility;
        public Visibility ImageVisibility
        {
            get { return imageVisibility; }
            set { imageVisibility = value;
                OnPropertyChanged();
            }
        }

        private string idText;
        public string IDText
        {
            get { return idText; }
            set { idText = value;
                OnPropertyChanged();
            }
        }

        private string source;
        public string Source
        {
            get { return source; }
            set { source = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClickLogin { get; set; }
        public ICommand CloseApp { get; set; }
        public ICommand MinimizeApp { get; set; }

        private void MainButtonClick()
        {
            user = Logger.Login(UserName, Password);
            Password = string.Empty;
            UserName = string.Empty;
            switch ((int)user.getPermission())
            {
                case 0:
                    break;
                case 1:
                    ImageVisibility = Visibility.Visible;
                    Source = "/Images/maid.png";
                    IDText = "ID: " + user.getId();
                    break;
                case 2:
                    ImageVisibility = Visibility.Visible;
                    Source = "/Images/bar.png";
                    IDText = "ID: " + user.getId();
                    break;
                case 3:
                    ImageVisibility = Visibility.Visible;
                    Source = "/Images/book.png";
                    IDText = "ID: " + user.getId();
                    break;
                case 4:
                    ImageVisibility = Visibility.Visible;
                    Source = "/Images/admin.png";
                    IDText = "ID: " + user.getId();
                    break;
            }
        }
        private void CloseAppClick()
        {
            App.Current.MainWindow.Close();
        }
        private void MinimizeAppClick()
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }


        public MainViewModel() 
        {
            ImageVisibility = Visibility.Collapsed;
            IDText = string.Empty;
            Source = "/Images/maid.png";

            user = new User();
            try
            {
                Logger.LoadData();
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }

            ClickLogin = new RelayCommand(o => MainButtonClick());
            CloseApp = new RelayCommand(o => CloseAppClick());
            MinimizeApp = new RelayCommand(o => MinimizeAppClick());

            HomeVM = new HomeViewModel();
            MaidVM = new MaidViewModel(this);
            BarVM = new BarViewModel(this);

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            MaidViewCommand = new RelayCommand(o =>
            {
                CurrentView = MaidVM;
            });

            BarViewCommand = new RelayCommand(o =>
            {
                CurrentView = BarVM;
            });
        }
    }
}
