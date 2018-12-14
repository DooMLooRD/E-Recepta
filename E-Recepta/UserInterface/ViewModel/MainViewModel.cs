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
        private static Uri _currentPage;
        private static bool _isLoggedIn = false;
        private bool _showLogs = false;

        public static Uri CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyStaticPropertyChanged("CurrentPage");
            }
        }

        public static bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyStaticPropertyChanged("IsLoggedIn");
            }
        }

        public static ObservableCollection<LoginAttemptDTO> LoginLogs
        {
            get => _loginLogs;
            set { _loginLogs = value; NotifyStaticPropertyChanged("LoginLogs");}
        }

        private static ObservableCollection<LoginAttemptDTO> _loginLogs;

        public MainViewModel()
        {
            //CurrentPage = new Uri("DoctorPage.Xaml", UriKind.RelativeOrAbsolute);
            //CurrentPage = new Uri("PharmacistPage.Xaml", UriKind.RelativeOrAbsolute);
            //CurrentPage = new Uri("PatientPage.Xaml", UriKind.RelativeOrAbsolute);
            CurrentPage = new Uri("LoginPage.Xaml", UriKind.RelativeOrAbsolute);
           
    }

        public bool ShowLogs
        {
            get => _showLogs;
            set { _showLogs = value; OnPropertyChanged(); }
        }

        public ICommand SwitchShowLogsCommand => new RelayCommand(() => ShowLogs = !ShowLogs, () => IsLoggedIn);
        public ICommand LogOutCommand => new RelayCommand(LogOut, () => IsLoggedIn);

        private void LogOut()
        {
            IsLoggedIn = false;
            CurrentPage = new Uri("LoginPage.Xaml", System.UriKind.RelativeOrAbsolute);
        }

        #region StaticPropertyChanged
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static async void LoginSuccessful(string username, string role)
        {
            
            CurrentPage = new Uri($"{role}Page.Xaml", System.UriKind.RelativeOrAbsolute);
            
            LoginService loginService = new LoginService();
            //await loginService.AddLoginAttempt(new LoginAttemptDTO()
            //{
            //    IsSuccessful = true,
            //    LoginTime = DateTime.Now,
            //    Username = username
            //});
            var logs = Task.Run(async () => await loginService.GetAllLoginAttempts(username));
            
        
            LoginLogs = new ObservableCollection<LoginAttemptDTO>(await logs);
            IsLoggedIn = true;
        }
    }
}