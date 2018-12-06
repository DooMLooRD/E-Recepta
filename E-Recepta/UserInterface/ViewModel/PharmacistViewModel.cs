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
    public class PharmacistViewModel : EmployeeViewModel
    {
        MedicinesInStockDB pharmacyDB = new MedicinesInStockDB("1");

        private List<UserDTO> _pharmacists;

        public PharmacistViewModel()
        {
            //GetPharmacyStateCommand.Execute(null);
            GetGeneralPharmacyStateCommand.Execute(null);
            
            
        }

        public MedicinesInStockDatabase.Medicine GeneralMedicineFilter { get; set; } = new MedicinesInStockDatabase.Medicine("", "", "");
        
        public MedicinesInStockDatabase.Medicine SelectedGeneralMedicine { get; set; }
        public MedicinesInStockDatabase.MedicineInStock InStockMedicineFilter { get; set; } = new MedicineInStock("", "", "", "0", "0", "0");



        public List<string> FileExtensions { get; set; } = new List<string>() {"Select file extension", "pdf", "csv"};
        public string SelectedFileExtension { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate{ get; set; }

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



        public ICommand GetPharmacistsSalesCommand => new RelayCommand(GetPharmacistsSales, () => SelectedPharmacist != null && SelectedFileExtension != FileExtensions[0]);
        public ICommand GetPatientsPrescriptionsCommand => new RelayCommand(GetPatientsPrescriptions, IsPrescriptionsDataReady);
        public ICommand LoadPatientsUnrealisedPrescriptionsCommand => new RelayCommand(GetPrescriptions, () => true);
        public ICommand AddToPharmacyCommand => new RelayCommand(AddToPharmacy, () => true);

        private void AddToPharmacy()
        {
            var medicine = new MedicineInStock(SelectedGeneralMedicine.Id, SelectedGeneralMedicine.Name, SelectedGeneralMedicine.Manufacturer, "1", "100", "1");
            PharmacyState.Add(medicine);
            GeneralPharmacyState.Remove(SelectedGeneralMedicine);
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
        public ICommand LoadPharmacistsCommand => new RelayCommand(async () =>
        {
            IsWorking = true;
            UserFilter.Role = "Pharmacist";
            UserFilter.Username = "";
            var users = await Task.Run(LoadUsers);
            Pharmacists = new List<UserDTO>(users);
            IsWorking = false;
        }, () => true);

        public ICommand GetPharmacyStateCommand => new RelayCommand(GetGeneralPharmacyState, () => true);
        public ICommand GetGeneralPharmacyStateCommand => new RelayCommand(GetGeneralPharmacyState, () => true);

        private async void GetGeneralPharmacyState()
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

        public ICommand UpdatePharmacyStateCommand => new RelayCommand(UpdatePharmacyState, () => true);

        private async void UpdatePharmacyState()
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
        }

        private int actualPharmacyMedicinesCount;
        private async void GetPharmacyState()
        {
            IsWorking = true;
            PharmacyState = new ObservableCollection<MedicineInStock>(await pharmacyDB.SearchMedicineInStock(InStockMedicineFilter.Name, InStockMedicineFilter.Manufacturer));
            actualPharmacyMedicinesCount = PharmacyState.Count;
            IsWorking = false;
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
                   SelectedFileExtension != String.Empty && EndDate >= StartDate && SelectedFileExtension != FileExtensions[0];
        }

        private async void GetPatientsPrescriptions()
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                var ext = ReportExt.CSV.ToString().ToLower() == SelectedFileExtension ? ReportExt.CSV : ReportExt.PDF;
                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                Generator.Generate(ReportType.PrescriptionsReport, ext, StartDate.Value, EndDate.Value, 1, loc);
            });
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