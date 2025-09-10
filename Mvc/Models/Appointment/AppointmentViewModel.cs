using System.ComponentModel.DataAnnotations;
using Hospital.Models.Doctor;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
using Hospital.Models.ValidationAttributes;

namespace Hospital.Models.Appointment
{
    public class AppointmentViewModel
    {
        [Required]
        [Display(Name = "ID Appointment")]
        public int ID { get; set; } // Non richiesto per la creazione, utile per l'editing

        [Display(Name = "ID Patient")]
        public int IDPatient { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } // Mostrato solo a schermo

        [Display(Name = "ID Nurse")]
        public int IDNurse { get; set; }

        [Display(Name = "Nurse Username")]
        public string NurseName { get; set; } // MOstrato solo a schermo

        [Display(Name = "ID Doctor")]
        public int IDDoctor { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } // Mostrato solo a schermo
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime Date { get; set; }
    }

    public class AppointmentEditPageViewModel
    {
        public List<PatientViewModel> Patients { get; set; } = new();
        public List<DoctorViewModel> Doctors { get; set; } = new();
        public List<NurseViewModel> Nurses { get; set; } = new();

        // Aggiungo tutti gli appuntamenti
        public List<AppointmentViewModel> Appointments { get; set; } = new();

        // Appuntamento selezionato da modificare
        public AppointmentCreateEditViewModel AppointmentToEdit { get; set; } = new();
    }

    public class AppointmentDeletePageViewModel
    {
        public List<PatientViewModel> Patients { get; set; } = new();
        public List<DoctorViewModel> Doctors { get; set; } = new();
        public List<NurseViewModel> Nurses { get; set; } = new();

        // Aggiungo tutti gli appuntamenti
        public List<AppointmentViewModel> Appointments { get; set; } = new();

        // Appuntamento selezionato da modificare
        public AppointmentCreateEditViewModel AppointmentToDelete { get; set; } = new();
    }
}
