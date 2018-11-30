using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using MedicinesInStockDatabase;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;

namespace UserInterface.ViewModel
{
    public class PharmacistViewModel : EmployeeViewModel
    {
        MedicinesInStockDB pharmacyDB = new MedicinesInStockDB("1");

        private List<UserDTO> _pharmacists;
        private ObservableCollection<MedicineInStock> _pharmacyState;
        public List<string> FileExtensions { get; set; } = new List<string>() {"pdf", "csv"};
        public string SelectedFileExtension { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate{ get; set; }

        public ObservableCollection<MedicineInStock> PharmacyState
        {
            get => _pharmacyState;
            set
            {
                _pharmacyState = value;
                OnPropertyChanged();
            }
        }

        public List<UserDTO> Pharmacists
        {
            get => _pharmacists;
            set
            {
                _pharmacists = value;
                OnPropertyChanged();
            }
        }

        public UserDTO SelectedPharmacist { get; set; }


        
        public DoctorViewModel.Prescription SelectedPatientsUnrealisedPrescription { get; set; }



        public ICommand GetPharmacistsSalesCommand => new RelayCommand(GetPharmacistsSales, () => SelectedPharmacist != null);
        public ICommand GetPatientsPrescriptionsCommand => new RelayCommand(GetPatientsPrescriptions, IsPrescriptionsDataReady);
        public ICommand LoadPatientsUnrealisedPrescriptionsCommand => new RelayCommand(GetPrescriptions, () => true);
        public ICommand LoadPatientsCommand => new RelayCommand(async () =>
        {
            IsWorking = true;
            UserFilter.Role = "Patient";
            UserFilter.Username = "";
            var x = Task.Run(async () => await LoadUsers());
            Patients = new List<UserDTO>(await x);
            IsWorking = false;
        }, () => true);
        public ICommand LoadPharmacistsCommand => new RelayCommand(async () =>
        {
            IsWorking = true;
            UserFilter.Role = "Pharmacist";
            UserFilter.Username = "";
            var x = Task.Run(async () => await LoadUsers());
            Pharmacists = new List<UserDTO>(await x);
            IsWorking = false;
        }, () => true);

        public ICommand GetPharmacyStateCommand => new RelayCommand(GetPharmacyState, () => true);
        public ICommand UpdatePharmacyStateCommand => new RelayCommand(UpdatePharmacyState, () => true);

        private void UpdatePharmacyState()
        {
            // todo
        }

        private async void GetPharmacyState()
        {
            PharmacyState = new ObservableCollection<MedicineInStock>(await pharmacyDB.SearchMedicineInStock("", ""));
        }


        public ICommand RealizePrescriptionCommand =>
            new RelayCommand(RealizePrescription, () => SelectedPatientsUnrealisedPrescription != null);

        private async void RealizePrescription()
        {
            //foreach (var medicine in SelectedPatientsUnrealisedPrescription.Medicines)
            //{
            //    //if(await pharmacyDB.SearchMedicineInStock(medicine.Medicine.Name, ""))
            //}
            //var xd = await pharmacyDB.SearchMedicineInStock("", "");
            IsWorking = true;
            await Task.Run(() => Thread.Sleep(1500));
            IsWorking = false;
        }

        

        private bool IsPrescriptionsDataReady()
        {
            return SelectedUser != null && StartDate != null && EndDate != null &&
                   SelectedFileExtension != String.Empty && EndDate >= StartDate;
        }

        private async void GetPatientsPrescriptions()
        {
            IsWorking = true;
            await Task.Run(() => Thread.Sleep(1500));
            IsWorking = false;
        }

        private async void GetPharmacistsSales()
        {
            IsWorking = true;
            await Task.Run(() => Thread.Sleep(1500));
            IsWorking = false;
        }


        
    }
}