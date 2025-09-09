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

        [Display(Name = "ID Nurse")]
        public int? IDNurse { get; set; } // opzionale

        [Display(Name = "Nurse Username")]
        public string? NurseName { get; set; } // opzionale

        [Display(Name = "ID Doctor")]
        public int IDDoctor { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } // Mostrato solo a schermo
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime Date { get; set; }

        [Display(Name ="Diagnosis")]
        public string? Diagnosis { get; set; }
        [Display(Name = "Prescription")]
        public string? Prescription { get; set; }
        [Display(Name = "Treatment")]
        public string? Treatment { get; set; }
    }
}
