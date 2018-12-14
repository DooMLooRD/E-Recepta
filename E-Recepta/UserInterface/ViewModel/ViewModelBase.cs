using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BlockChain;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;
using BlockChain = BlockChain.BlockChain;

namespace UserInterface.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public static BlockChainHandler blockChainHandler = new BlockChainHandler();

        protected ViewModelBase()
        {

        }

        private bool isWorking;
        public event PropertyChangedEventHandler PropertyChanged;

        protected UserDTO selectedUser;
        public UserDTO SelectedUser
        {
            get => selectedUser;
            set { selectedUser = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsWorking
        {
            get => isWorking;
            set
            {
                isWorking = value;
                OnPropertyChanged();
            }
        }




        public List<DoctorViewModel.Prescription> Prescriptions { get; set; }
        protected async void GetPrescriptions()
        {
            IsWorking = true;
            await Task.Run(() =>
            {

                DoctorViewModel.PrescriptionMedicine pm = new DoctorViewModel.PrescriptionMedicine(new MedicinesDatabase.Medicine("a", "lek1", "man1", "1"));
                pm.Amount = 2;
                DoctorViewModel.PrescriptionMedicine pm2 = new DoctorViewModel.PrescriptionMedicine(new MedicinesDatabase.Medicine("b", "lek2", "man2", "2"));
                pm.Amount = 4;
                Prescriptions = new List<DoctorViewModel.Prescription>()
                {
                    new DoctorViewModel.Prescription()
                    {
                        Medicines = new ObservableCollection<DoctorViewModel.PrescriptionMedicine>()
                        {
                            pm, pm2
                        },
                        Doctor = new UserDTO()
                        {
                            LastName = "abc"
                        },
                        Date = DateTime.Now
                    },
                    new DoctorViewModel.Prescription()
                    {
                        Medicines = new ObservableCollection<DoctorViewModel.PrescriptionMedicine>()
                        {
                            pm, pm2
                        },
                        Doctor = new UserDTO()
                        {
                            LastName = "cba"
                        }
                    },
                };
                OnPropertyChanged("Prescriptions");

            });
            IsWorking = false;
        }
    }
}