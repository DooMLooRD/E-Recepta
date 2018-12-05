using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Authorisation;
using UserInterface.Command;
using UserInterface.View;

namespace UserInterface.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<string> Roles => new List<string> { "Patient", "Pharmacist", "Doctor" };


        public ICommand LoginCommand => new RelayCommand(SignIn, VerifyData);
        private bool VerifyData()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Role))
                return false;
            return true;
        }

        private async void SignIn()
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                UserAuthorisation userAuthorisation = new UserAuthorisation();
                try
                {
                    if (userAuthorisation.CreateSession(Username, Password, Role) != String.Empty)
                    {
                        MainViewModel.LoginSuccessful(Username, Role);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
            IsWorking = false;

            
        }
    }
}