using System.ComponentModel;

namespace Hospital.Models.Patient
{
    public class PatientViewModel
    {
        public int ID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        public string Surname { get; set; }

        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Local Address")]
        public string Address { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }
    }

    public class PatientEditPageViewModel
    {
        // Lista dei dottori per la tendina
        public List<PatientViewModel> Patients { get; set; } = new();

        // Doctor selezionato e modificabile
        public PatientEditViewModel PatientToEdit { get; set; } = new();
    }

    public class PatientDeletePageViewModel
    {
        // Lista dei dottori per la tendina
        public List<PatientViewModel> Patients { get; set; } = new();

        // Doctor selezionato e modificabile
        public PatientEditViewModel PatientToDelete { get; set; } = new();
    }
}
