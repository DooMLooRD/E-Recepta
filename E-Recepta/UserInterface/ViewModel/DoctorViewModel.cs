using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MedicinesDatabase;
using MedicinesInStockDatabase;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;
using Medicine = BlockChain.Medicine;

namespace UserInterface.ViewModel
{
    public class DoctorViewModel : EmployeeViewModel
    {
        private ObservableCollection<MedicinesDatabase.Medicine> _medicines;
        private ObservableCollection<PrescriptionMedicine> _newPrescription = new ObservableCollection<PrescriptionMedicine>();

        public MedicinesDatabase.Medicine SelectedMedicine { get; set; }
        public MedicinesDatabase.Medicine MedicineFilter { get; set; } = new MedicinesDatabase.Medicine("", "", "", "0");
        public PrescriptionMedicine SelectedPrescriptionMedicine { get; set; }
        public ObservableCollection<MedicinesDatabase.Medicine> Medicines
        {
            get => _medicines;
            set { _medicines = value; OnPropertyChanged(); }
        }
        public ObservableCollection<DoctorViewModel.Prescription> DoctorsPrescriptions { get; set; } = new ObservableCollection<Prescription>();

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
        public ICommand CreatePrescriptionCommand => new RelayCommand(CreatePrescription, () => NewPrescription.Any() && SelectedUser != null);
        public ICommand LoadDoctorsPrescriptionsCommand => new RelayCommand(GetDoctorsPrescriptions, () => true);

        private async void GetDoctorsPrescriptions()
        {
            DoctorsPrescriptions.Clear();
            var allPrescriptions = blockChainHandler.GetAllPrescriptionsByDoctor("12");
            if (allPrescriptions == null)
            {
                return;
            }

            foreach (var prescription in allPrescriptions)
            {
                DoctorsPrescriptions.Add(new Prescription
                {
                    Date = prescription.Date,
                    ValidSince = prescription.ValidSince,
                    Patient = await userService.GetUser(Convert.ToInt32(prescription.patientId))
                });
                foreach (var prescriptionMedicine in prescription.medicines)
                {
                    var medicine = (await medicineModule.SearchMedicineById(prescriptionMedicine.id.ToString())).SingleOrDefault();

                    if (medicine != null)
                    {
                        DoctorsPrescriptions.Last().Medicines.Add(new PrescriptionMedicine(medicine));
                        DoctorsPrescriptions.Last().Medicines.Last().Amount = prescriptionMedicine.amount;
                    }
                }
            }
            OnPropertyChanged("DoctorsPrescriptions");
        }

        private async void CreatePrescription()
        {
           
            IsWorking = true;
            //await Task.Run(() =>
            {
                var medicines = new ObservableCollection<Medicine>();
                foreach (var prescriptionMedicine in NewPrescription)
                {
                    medicines.Add(new Medicine(Convert.ToInt32(prescriptionMedicine.Medicine.Id),
                        prescriptionMedicine.Amount));
                }
                //ObservableCollection<Medicine> medicines = new ObservableCollection<Medicine>();
                //Medicine medicine = new Medicine(2, 5);
                //Medicine medicine2 = new Medicine(1, 1);
                //medicines.Add(medicine);
                //medicines.Add(medicine2);
                blockChainHandler.AddPrescription(selectedUser.Id.ToString(), "12", medicines);

                
            }//);
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
            public UserDTO Doctor { get; set; }
            public UserDTO Patient { get; set; }
            public bool Realised { get; set; }
            public ObservableCollection<PrescriptionMedicine> Medicines { get; set; } = new ObservableCollection<PrescriptionMedicine>();
        }

        public class PrescriptionMedicine
        {
            public int Amount { get; set; }

            public MedicinesDatabase.Medicine Medicine { get; set; }

            public int InStockAmount { get; set; }
            public PrescriptionMedicine(MedicinesDatabase.Medicine medicine)
            {
                Medicine = medicine;
                Amount = 1;
            }

            public PrescriptionMedicine()
            {

            }
        }
    }
}