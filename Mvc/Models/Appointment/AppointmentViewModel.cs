using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "ID Doctor")]
        public int IDDoctor { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } // Mostrato solo a schermo

        [Display(Name = "Date and Time")]
        public DateTime Date { get; set; }
    }
}
