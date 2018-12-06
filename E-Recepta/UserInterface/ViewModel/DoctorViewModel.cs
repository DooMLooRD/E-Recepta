using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MedicinesDatabase;
using MedicinesInStockDatabase;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;

namespace UserInterface.ViewModel
{
    public class DoctorViewModel : EmployeeViewModel
    {
        private MedicinesDB medicineModule;
        private ObservableCollection<MedicinesDatabase.Medicine> _medicines;
        private ObservableCollection<PrescriptionMedicine> _newPrescription = new ObservableCollection<PrescriptionMedicine>();

        public MedicinesDatabase.Medicine SelectedMedicine { get; set; }
        public MedicinesDatabase.Medicine MedicineFilter { get; set; } = new MedicinesDatabase.Medicine("", "", "", "0");
        public PrescriptionMedicine SelectedPrescriptionMedicine { get; set; }
        public ObservableCollection<MedicinesDatabase.Medicine> Medicines
        {
            get => _medicines;
            set { _medicines = value; OnPropertyChanged();}
        }

        public ObservableCollection<PrescriptionMedicine> NewPrescription
        {
            get => _newPrescription;
            set => _newPrescription = value;
        }

        
        public ICommand LoadPatientsCommand => new RelayCommand(async () =>
        {
            IsWorking = true;
            UserFilter.Role = "Patient";
            UserFilter.Username = "";
            var x = Task.Run(async () => await LoadUsers());
            Patients = new List<UserDTO>(await x);
            IsWorking = false;
        }, () => true);

        public ICommand AddToPrescriptionCommand => new RelayCommand(AddToPrescription, () => true);
        public ICommand RemoveFromPrescriptionCommand => new RelayCommand(RemoveFromPrescription, () => true);
        public ICommand RealizePrescriptionCommand => new RelayCommand(RealizePrescription, () => NewPrescription.Any() && SelectedUser!=null);
        public ICommand LoadDoctorsPrescriptionsCommand => new RelayCommand(GetPrescriptions, () => true);

        private async void RealizePrescription()
        {
            IsWorking = true;
            await Task.Run(() => Thread.Sleep(1000));
            GetPrescriptions();
            IsWorking = false;
        }

        private void RemoveFromPrescription()
        {
            Medicines.Add(SelectedPrescriptionMedicine.Medicine);
            NewPrescription.Remove(SelectedPrescriptionMedicine);
        }

        private void AddToPrescription()
        {
            NewPrescription.Add(new PrescriptionMedicine(SelectedMedicine));
            Medicines.Remove(SelectedMedicine);
        }

        public DoctorViewModel()
        {
            medicineModule = new MedicinesDB();
            LoadDoctorsPrescriptionsCommand.Execute(null);
        }
        public ICommand LoadMedicinesCommand => new RelayCommand(LoadMedicines, () => true);

        private async void LoadMedicines()
        {
            IsWorking = true;
            var medicines = await medicineModule.SearchMedicine(MedicineFilter.Name, MedicineFilter.Manufacturer);
            Medicines = new ObservableCollection<MedicinesDatabase.Medicine>(medicines);
            IsWorking = false;
        }

        public class Prescription
        {
            public string Id { get; set; }
            public DateTime Date { get; set; }
            public DateTime ValidSince { get; set; }
            public User Doctor { get; set; }

            public ObservableCollection<PrescriptionMedicine> Medicines { get; set; }
        }

        public class PrescriptionMedicine
        {
            public int Amount { get; set; }

            public MedicinesDatabase.Medicine Medicine { get; set; }

            public PrescriptionMedicine(MedicinesDatabase.Medicine medicine)
            {
                Medicine = medicine;
                Amount = 1;
            }
        }
    }
}