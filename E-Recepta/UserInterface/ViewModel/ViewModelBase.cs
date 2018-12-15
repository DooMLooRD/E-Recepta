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
using MedicinesDatabase;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;
using BlockChain = BlockChain.BlockChain;

namespace UserInterface.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public static BlockChainHandler blockChainHandler = new BlockChainHandler();
        protected UserService userService = new UserService();
        protected MedicinesDB medicineModule = new MedicinesDB();

        private bool isWorking;
        public bool IsWorking
        {
            get => isWorking;
            set
            {
                isWorking = value;
                OnPropertyChanged();
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}