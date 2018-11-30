using System.Windows.Input;
using UserInterface.Command;

namespace UserInterface.ViewModel
{
    public class PatientViewModel : ViewModelBase
    {
        public ICommand LoadPatientsPrescriptionsCommand => new RelayCommand(GetPrescriptions, () => true);
    }
}