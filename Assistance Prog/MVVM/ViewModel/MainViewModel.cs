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
        public RelayCommand DiscoveryViewCommand { get; set; }

        public User user { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }

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

        public ICommand ClickLogin { get; set; }

        private void MainButtonClick()
        {
            MessageBox.Show("Password = " + password + " Login = " + userName);
        }


        public MainViewModel() 
        { 
            user = new User();
            try
            {
                //Logger.LoadData();
            } catch (DirectoryNotFoundException ex) 
            {
                MessageBox.Show(ex.Message);
            }

            ClickLogin = new RelayCommand(o => MainButtonClick());

            HomeVM = new HomeViewModel();
            DiscoveryVM = new DiscoveryViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = DiscoveryVM;
            });
        }
    }
}
