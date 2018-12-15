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
    public class DoctorViewModel : ViewModelBase
    {
        public DoctorViewModel()
        {
            LoadDoctorsPrescriptionsCommand.Execute(null);
        }

        public MedicinesDatabase.Medicine SelectedNFZMedicine { get; set; }
        public MedicinesDatabase.Medicine MedicineFilter { get; set; } = new MedicinesDatabase.Medicine("", "", "", "0");
        public UserDTO PatientFilter { get; set; } = new UserDTO();
        public PrescriptionMedicine SelectedPrescriptionMedicine { get; set; }
        public ObservableCollection<DoctorViewModel.Prescription> DoctorsPrescriptions { get; set; } = new ObservableCollection<Prescription>();
        public ObservableCollection<PrescriptionMedicine> NewPrescription { get; set; } = new ObservableCollection<PrescriptionMedicine>();

        private UserDTO _selectedPatient;
        public UserDTO SelectedPatient
        {
            get => _selectedPatient;
            set { _selectedPatient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MedicinesDatabase.Medicine> _nfzMedicines;
        public ObservableCollection<MedicinesDatabase.Medicine> NFZMedicines
        {
            get => _nfzMedicines;
            set { _nfzMedicines = value; OnPropertyChanged(); }
        }

        private List<UserDTO> _patients;
        public List<UserDTO> Patients
        {
            get => _patients;
            set { _patients = value; OnPropertyChanged(); }
        }

        public ICommand LoadPatientsCommand => new RelayCommand(LoadPatients, () => true);
        public ICommand AddToPrescriptionCommand => new RelayCommand(AddToPrescription, () => true);
        public ICommand RemoveFromPrescriptionCommand => new RelayCommand(RemoveFromPrescription, () => true);
        public ICommand CreatePrescriptionCommand => new RelayCommand(CreatePrescription, () => NewPrescription.Any() && SelectedPatient != null);
        public ICommand LoadDoctorsPrescriptionsCommand => new RelayCommand(GetDoctorsPrescriptions, () => true);
        public ICommand LoadMedicinesCommand => new RelayCommand(LoadMedicines, () => true);


        private async void LoadPatients()
        {
            IsWorking = true;
            PatientFilter.Role = "Patient";
            PatientFilter.Username = "";
            var users = await userService.GetUsers(PatientFilter.Name, PatientFilter.LastName,
                PatientFilter.Pesel, PatientFilter.Role, PatientFilter.Username);
            Patients = new List<UserDTO>(users);
            IsWorking = false;
        }

        private async void GetDoctorsPrescriptions()
        {
            DoctorsPrescriptions.Clear();
            var allPrescriptions = blockChainHandler.GetAllPrescriptionsByDoctor("12");
            if (allPrescriptions == null)
            {
                MessageBox.Show("Blockchain unavailable, signing out..");
                MainViewModel.LogOut();
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

                if (!blockChainHandler.AddPrescription(_selectedPatient.Id.ToString(), "12", medicines))
                {
                    MessageBox.Show("Blockchain unavailable, signing out..");
                    MainViewModel.LogOut();
                }
                else MessageBox.Show("Prescription added");


            }//);
            
            GetDoctorsPrescriptions();
            IsWorking = false;
        }

        private void RemoveFromPrescription()
        {
            NFZMedicines.Add(SelectedPrescriptionMedicine.Medicine);
            NewPrescription.Remove(SelectedPrescriptionMedicine);
        }

        private void AddToPrescription()
        {
            NewPrescription.Add(new PrescriptionMedicine(SelectedNFZMedicine));
            NFZMedicines.Remove(SelectedNFZMedicine);
        }

        private async void LoadMedicines()
        {
            IsWorking = true;
            NewPrescription.Clear();
            var medicines = await medicineModule.SearchMedicine(MedicineFilter.Name, MedicineFilter.Manufacturer);
            NFZMedicines = new ObservableCollection<MedicinesDatabase.Medicine>(medicines);
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