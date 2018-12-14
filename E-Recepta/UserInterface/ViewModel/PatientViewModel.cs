using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UserInterface.Command;

namespace UserInterface.ViewModel
{
    public class PatientViewModel : ViewModelBase
    {
        public ICommand LoadPatientsPrescriptionsCommand => new RelayCommand(GetUsersPrescriptions, () => true);
        public ObservableCollection<DoctorViewModel.Prescription> PatientsPrescriptions { get; set; } = new ObservableCollection<DoctorViewModel.Prescription>();
        private async void GetUsersPrescriptions()
        {
            
            PatientsPrescriptions.Clear();
            var allPrescriptions = blockChainHandler.GetAllPrescriptionsByPatient("16");
            var realisedPrescriptions = blockChainHandler.GetAllRealizedPrescriptionsByPatient("16");


            foreach (var prescription in allPrescriptions)
            {
                PatientsPrescriptions.Add(new DoctorViewModel.Prescription
                {
                    Date = prescription.Date,
                    ValidSince = prescription.ValidSince,
                    Doctor = await userService.GetUser(Convert.ToInt32(prescription.doctorId)),

                    Id = prescription.prescriptionId
                });
                if (realisedPrescriptions.Select(x => x.prescriptionId).Contains(prescription.prescriptionId))
                {
                    PatientsPrescriptions.Last().Realised = true;
                }
                else
                    PatientsPrescriptions.Last().Realised = false;
                foreach (var prescriptionMedicine in prescription.medicines)
                {
                    var medicine = (await medicineModule.SearchMedicineById(prescriptionMedicine.id.ToString())).SingleOrDefault();

                    if (medicine != null)
                    {
                        PatientsPrescriptions.Last().Medicines.Add(new DoctorViewModel.PrescriptionMedicine(medicine));
                        PatientsPrescriptions.Last().Medicines.Last().Amount = prescriptionMedicine.amount;
                    }
                }
            }

            OnPropertyChanged("PatientsPrescriptions");
        }

        public PatientViewModel()
        {
            //LoadPatientsPrescriptionsCommand.Execute(null);
        }
    }


}