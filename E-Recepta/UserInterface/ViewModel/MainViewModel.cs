using BlockChain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using UserDatabaseAPI.Service;
using UserInterface.Command;
using UserInterface.View;

namespace UserInterface.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Task.Run(() => ViewModelBase.blockChainHandler.InitializeBlockChains());

            //CurrentPage = new Uri("DoctorPage.Xaml", UriKind.RelativeOrAbsolute);
            //CurrentPage = new Uri("PharmacistPage.Xaml", UriKind.RelativeOrAbsolute);
            //CurrentPage = new Uri("PatientPage.Xaml", UriKind.RelativeOrAbsolute);
            CurrentPage = new Uri("LoginPage.Xaml", UriKind.RelativeOrAbsolute);

        }

        private static Uri _currentPage;
        public static Uri CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyStaticPropertyChanged("CurrentPage");
            }
        }

        private static bool _isLoggedIn = false;
        public static bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyStaticPropertyChanged("IsLoggedIn");
            }
        }

        private static ObservableCollection<LoginAttemptDTO> _loginLogs;
        public static ObservableCollection<LoginAttemptDTO> LoginLogs
        {
            get => _loginLogs;
            set { _loginLogs = value; NotifyStaticPropertyChanged("LoginLogs");}
        }

        private bool _showLogs = false;
        public bool ShowLogs
        {
            get => _showLogs;
            set { _showLogs = value; OnPropertyChanged(); }
        }


        public ICommand SwitchShowLogsCommand => new RelayCommand(() => ShowLogs = !ShowLogs, () => IsLoggedIn);
        public ICommand LogOutCommand => new RelayCommand(LogOut, () => IsLoggedIn);

        public static void LogOut()
        {
            IsLoggedIn = false;
            CurrentPage = new Uri("LoginPage.Xaml", System.UriKind.RelativeOrAbsolute);
        }

        public static async void LoginSuccessful(string username, string role)
        {
            CurrentPage = new Uri($"{role}Page.Xaml", System.UriKind.RelativeOrAbsolute);
            
            LoginService loginService = new LoginService();
            var logs = Task.Run(async () => await loginService.GetAllLoginAttempts(username));
            LoginLogs = new ObservableCollection<LoginAttemptDTO>(await logs);
            IsLoggedIn = true;
        }

        #region PropertyChangedHandlers
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}