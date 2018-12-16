using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Authorisation;
using BlockChain;
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
                    if (!CheckInternetConnection())
                    {
                        MessageBox.Show("No internet connection");
                        return;
                    }
                    if (!blockChainHandler.IsBlockChainAvailable())
                    {
                        MessageBox.Show("Blockchain unavailable");
                        return;
                    }

                    CurrentUserId = userAuthorisation.CreateSession(Username, Password, Role);
                    if (CurrentUserId != 0)
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

        private bool CheckInternetConnection()
        {
            try
            {
                string URL = "http://google.com";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}