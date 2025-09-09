using System.ComponentModel.DataAnnotations;
using Hospital.Models.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Models.Appointment
{
    public class AppointmentCreateEditViewModel
    {
        [Display(Name = "ID Appointment")]
        public int ID { get; set; } // Non richiesto per la creazione, utile per l'editing

        [Required(ErrorMessage = "Patient ID is required")]
        [Display(Name = "ID Patient")]
        public int IDPatient { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        [Display(Name = "ID Doctor")]
        public int IDDoctor { get; set; }

        [Required(ErrorMessage = "Date and time are required")]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "The appointment date must be in the future")]
        public DateTime Date { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department is required")]
        [ValidDepartment(ErrorMessage = "Dipartimento non valido. Valori ammessi: Cardiology, Neurology, Pediatrics, Emergency, General")]
        public string Department { get; set; }

        [Display(Name = "Diagnosis")]
        public string? Diagnosis { get; set; }
        [Display(Name = "Prescription")]
        public string? Prescription { get; set; }
        [Display(Name = "Treatment")]
        public string? Treatment { get; set; }

        // Liste di supporto per le dropdown (mostro il nome utente, invio l'id)
        public List<SelectListItem> Patients { get; set; } = new();
        public List<SelectListItem> Doctors { get; set; } = new();

    }


    public class ValidDepartmentAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return Enum.GetNames(typeof(Models.Enums.Department)).Contains(value.ToString());
        }
    }
}
