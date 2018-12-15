using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MedicinesDatabase;
using MedicinesInStockDatabase;
using ReportGenerator;
using UserDatabaseAPI.Service;
using UserDatabaseAPI.UserDB.Entities;
using UserInterface.Command;
using Medicine = MedicinesInStockDatabase.Medicine;

namespace UserInterface.ViewModel
{
    public class PharmacistViewModel : ViewModelBase
    {
        public PharmacistViewModel()
        {
            GetGeneralPharmacyStateCommand.Execute(null);
        }
        private int actualPharmacyMedicinesCount;
        MedicinesInStockDB pharmacyDB = new MedicinesInStockDB("1");
        public MedicinesInStockDatabase.Medicine GeneralMedicineFilter { get; set; } = new MedicinesInStockDatabase.Medicine("", "", "");
        public MedicinesInStockDatabase.Medicine SelectedGeneralMedicine { get; set; }
        public MedicinesInStockDatabase.MedicineInStock InStockMedicineFilter { get; set; } = new MedicineInStock("", "", "", "0", "0", "0");
        public List<string> FileExtensions { get; set; } = new List<string>() { "Select file extension", "pdf", "csv" };
        public string SelectedFileExtension { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserDTO SelectedPharmacist { get; set; }
        public UserDTO PatientFilter { get; set; } = new UserDTO();
        public UserDTO PharmacistFilter { get; set; } = new UserDTO();
        public DoctorViewModel.Prescription SelectedPatientsUnrealisedPrescription { get; set; }
        public ObservableCollection<DoctorViewModel.Prescription> SelectedPatientsUnrealisedPrescriptions { get; set; }

        private List<UserDTO> _pharmacists;
        public List<UserDTO> Pharmacists
        {
            get => _pharmacists;
            set
            {
                _pharmacists = value;
                OnPropertyChanged();
            }
        }

        private UserDTO _selectedPatient;
        public UserDTO SelectedPatient
        {
            get => _selectedPatient;
            set { _selectedPatient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MedicineInStock> _pharmacyState;
        public ObservableCollection<MedicineInStock> PharmacyState
        {
            get => _pharmacyState;
            set
            {
                _pharmacyState = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Medicine> _generalPharmacyState;
        public ObservableCollection<Medicine> GeneralPharmacyState
        {
            get => _generalPharmacyState;
            set
            {
                _generalPharmacyState = value;
                OnPropertyChanged();
            }
        }

        private List<UserDTO> _patients;
        public List<UserDTO> Patients
        {
            get => _patients;
            set { _patients = value; OnPropertyChanged(); }
        }

        public ICommand AddToPharmacyCommand => new RelayCommand(AddToPharmacy, () => true);
        public ICommand LoadPatientsCommand => new RelayCommand(LoadPatients, () => true);
        public ICommand LoadPharmacistsCommand => new RelayCommand(LoadPharmacists, () => true);
        public ICommand GetGeneralPharmacyStateCommand => new RelayCommandAsync(GetGeneralPharmacyState, () => true);
        public ICommand UpdatePharmacyStateCommand => new RelayCommandAsync(UpdatePharmacyState, () => true);
        public ICommand RealizePrescriptionCommand => new RelayCommand(RealizePrescription, CanBeRealised);
        public ICommand GetPharmacistsSalesCommand => new RelayCommand(GetPharmacistsSales, () => SelectedPharmacist != null && SelectedFileExtension != FileExtensions[0]);
        public ICommand GetPatientsPrescriptionsCommand => new RelayCommand(GetPatientsPrescriptions, IsPrescriptionsDataReady);
        public ICommand LoadPatientsUnrealisedPrescriptionsCommand => new RelayCommand(GetPatientsUnrealisedPrescriptions, () => true);

        private async void GetPatientsUnrealisedPrescriptions()
        {
            if (SelectedPatient == null)
                return;
            IsWorking = true;
            await Task.Run(async () =>
            {
                SelectedPatientsUnrealisedPrescriptions = new ObservableCollection<DoctorViewModel.Prescription>();
                var allPrescriptions = blockChainHandler.GetAllPrescriptionsByPatient(_selectedPatient.Id.ToString());
                var realizedPrescriptions = blockChainHandler.GetAllRealizedPrescriptionsByPatient(SelectedPatient.Id.ToString());
                var unrealizedPrescriptions = new ObservableCollection<BlockChain.Prescription>();

                foreach (var prescription in allPrescriptions)
                {
                    if (!realizedPrescriptions.Select(x => x.prescriptionId).Contains(prescription.prescriptionId))
                        unrealizedPrescriptions.Add(prescription);
                }

                foreach (var prescription in unrealizedPrescriptions)
                {
                    var medicines = new ObservableCollection<DoctorViewModel.PrescriptionMedicine>();
                    foreach (var prescriptionMedicine in prescription.medicines)
                    {
                        medicines.Add(new DoctorViewModel.PrescriptionMedicine
                        {
                            Amount = prescriptionMedicine.amount,
                            Medicine = (await medicineModule.SearchMedicineById(prescriptionMedicine.id.ToString())).Single(),

                        });
                        var actualMedicine = PharmacyState.SingleOrDefault(x => x.Name == medicines.Last().Medicine.Name);
                        medicines.Last().InStockAmount = actualMedicine == null ? 0 : actualMedicine.Amount;
                    }
                    SelectedPatientsUnrealisedPrescriptions.Add(new DoctorViewModel.Prescription
                    {
                        Date = prescription.Date,
                        Doctor = await userService.GetUser(Convert.ToInt32(prescription.doctorId)),
                        Id = prescription.prescriptionId,
                        ValidSince = prescription.ValidSince,
                        Medicines = medicines
                    });

                }

                OnPropertyChanged("SelectedPatientsUnrealisedPrescriptions");
            });
            IsWorking = false;
        }

        private void AddToPharmacy()
        {
            var medicine = new MedicineInStock(SelectedGeneralMedicine.Id, SelectedGeneralMedicine.Name, SelectedGeneralMedicine.Manufacturer, "1", "100", "1");
            PharmacyState.Add(medicine);
            GeneralPharmacyState.Remove(SelectedGeneralMedicine);
        }

        private async void LoadPatients()
        {
            IsWorking = true;
            SelectedPatientsUnrealisedPrescriptions?.Clear();
            OnPropertyChanged("SelectedPatientsUnrealisedPrescriptions");
            if (SelectedPatient != null)
                SelectedPatient = null;
            PatientFilter.Role = "Patient";
            PatientFilter.Username = "";
            var users = await userService.GetUsers(PatientFilter.Name, PatientFilter.LastName,
                PatientFilter.Pesel, PatientFilter.Role, PatientFilter.Username);
            Patients = new List<UserDTO>(users);
            IsWorking = false;
        }

        private async void LoadPharmacists()
        {
            IsWorking = true;
            PharmacistFilter.Role = "Pharmacist";
            PharmacistFilter.Username = "";
            var users = await userService.GetUsers(PharmacistFilter.Name, PharmacistFilter.LastName,
                PharmacistFilter.Pesel, PharmacistFilter.Role, PharmacistFilter.Username);
            Pharmacists = new List<UserDTO>(users);
            IsWorking = false;
        }

        private async Task GetGeneralPharmacyState()
        {
            IsWorking = true;
            PharmacyState = new ObservableCollection<MedicineInStock>(await pharmacyDB.SearchMedicineInStock(InStockMedicineFilter.Name, InStockMedicineFilter.Manufacturer));
            actualPharmacyMedicinesCount = PharmacyState.Count;
            GeneralPharmacyState = new ObservableCollection<Medicine>(await pharmacyDB.SearchMedicine(GeneralMedicineFilter.Name, GeneralMedicineFilter.Manufacturer));
            var temp = (from x in PharmacyState
                      join y in GeneralPharmacyState
                          on x.Id equals y.Id
                      select y).ToList();
            GeneralPharmacyState = new ObservableCollection<Medicine>(GeneralPharmacyState.Except(temp));
            IsWorking = false;
        }

        private async Task UpdatePharmacyState()
        {
            for (int i = 0; i < actualPharmacyMedicinesCount; i++)
            {
                var medicine = PharmacyState[i];
                if (medicine.Amount == 0)
                {
                    await pharmacyDB.RemoveMedicineFromStock(medicine.Id);
                }
                else
                {
                    await pharmacyDB.UpdateMedicineAmountInStock(medicine.Id, medicine.Amount.ToString(),
                        medicine.Cost.ToString("G", CultureInfo.InvariantCulture));
                }
            }
            for (int i = actualPharmacyMedicinesCount; i < PharmacyState.Count; i++)
            {
                var medicine = PharmacyState[i];
                if(medicine.Amount == 0)
                    continue;
                await pharmacyDB.AddMedicineToStock(medicine.Id, medicine.Amount.ToString(), medicine.Cost.ToString("G", CultureInfo.InvariantCulture));
            }
            GetGeneralPharmacyStateCommand.Execute(null);
            GetPatientsUnrealisedPrescriptions();
        }

        private async void GetPharmacyState()
        {
            IsWorking = true;
            PharmacyState = new ObservableCollection<MedicineInStock>(await pharmacyDB.SearchMedicineInStock(InStockMedicineFilter.Name, InStockMedicineFilter.Manufacturer));
            actualPharmacyMedicinesCount = PharmacyState.Count;
            IsWorking = false;
        }

        private bool CanBeRealised()
        {
            if (SelectedPatientsUnrealisedPrescription == null)
                return false;
            if (SelectedPatientsUnrealisedPrescription.ValidSince > DateTime.Now)
                return false;
            foreach (var medicine in SelectedPatientsUnrealisedPrescription.Medicines)
            {
                if (medicine.Amount > medicine.InStockAmount)
                    return false;
            }

            return true;
        }

        private async void RealizePrescription()
        {
            IsWorking = true;
            await Task.Run(async () =>
            {

                if (!blockChainHandler.RealizePrescription(SelectedPatientsUnrealisedPrescription.Id, "1")) // id_zalogowanego
                {
                    MessageBox.Show("Blockchain unavailable, signing out..");
                    MainViewModel.LogOut();
                }

                await GetGeneralPharmacyState();

                foreach (var prescriptionMedicine in SelectedPatientsUnrealisedPrescription.Medicines)
                {
                    PharmacyState.SingleOrDefault(x => x.Name == prescriptionMedicine.Medicine.Name).Amount -=
                        prescriptionMedicine.Amount;
                }

                await UpdatePharmacyState();
                //SelectedPatientsUnrealisedPrescriptions.Remove(SelectedPatientsUnrealisedPrescription);
                OnPropertyChanged("SelectedPatientsUnrealisedPrescriptions");
                //GetPatientsUnrealisedPrescriptions();
            });
            IsWorking = false;
        }

        private bool IsPrescriptionsDataReady()
        {
            return SelectedPatient != null && StartDate != null && EndDate != null &&
                   SelectedFileExtension != String.Empty && EndDate >= StartDate && SelectedFileExtension != FileExtensions[0];
        }

        private async void GetPatientsPrescriptions()
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                if (!blockChainHandler.IsBlockChainAvailable())
                {
                    MessageBox.Show("Blockchain unavailable, signing out..");
                    MainViewModel.LogOut();
                }
                var ext = ReportExt.CSV.ToString().ToLower() == SelectedFileExtension ? ReportExt.CSV : ReportExt.PDF;
                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                Generator.Generate(ReportType.PrescriptionsReport, ext, StartDate.Value, EndDate.Value, 1, loc);
            });
            IsWorking = false;
        }

        private async void GetPharmacistsSales()
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                if (!blockChainHandler.IsBlockChainAvailable())
                {
                    MessageBox.Show("Blockchain unavailable, signing out..");
                    MainViewModel.LogOut();
                }
            });
            IsWorking = false;
        }   
    }
}