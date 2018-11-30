using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;

namespace UserInterface.ViewModel
{
    public abstract class EmployeeViewModel : ViewModelBase
    {
        protected UserService userService = new UserService();
        protected List<UserDTO> _patients;
        public List<UserDTO> Patients
        {
            get => _patients;
            set { _patients = value; OnPropertyChanged(); }
        }


        public UserDTO UserFilter { get; set; } = new UserDTO();
        protected async Task<IEnumerable<UserDTO>> LoadUsers()
        {
            return await userService.GetUsers(UserFilter.Name, UserFilter.LastName,
                UserFilter.Pesel, UserFilter.Role, UserFilter.Username);
            //IsWorking = true;
            //Task.Run(async () =>
            //{
            //    return (userService.GetUsers(UserFilter.Name, UserFilter.LastName,
            //        UserFilter.Pesel, UserFilter.Role, UserFilter.Username));
            //});
            // IsWorking = false;
        }
    }
}