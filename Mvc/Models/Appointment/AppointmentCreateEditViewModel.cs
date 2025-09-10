using System.ComponentModel.DataAnnotations;
using Hospital.Models.Doctor;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
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
        
        [Required(ErrorMessage = "Nurse ID is required")]
        [Display(Name = "ID Nurse")]
        public int IDNurse { get; set; }

        [Required(ErrorMessage = "Date and time are required")]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "The appointment date must be in the future")]
        public DateTime Date { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department is required")]
        [ValidDepartment(ErrorMessage = "Dipartimento non valido. Valori ammessi: Cardiology, Neurology, Pediatrics, Emergency, General")]
        public string Department { get; set; }
    }


    public class ValidDepartmentAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return Enum.GetNames(typeof(Models.Enums.Department)).Contains(value.ToString());
        }
    }


    public class CreateAppointmentViewModel
    {
        public List<PatientViewModel> Patients { get; set; } = new();
        public List<DoctorViewModel> Doctors { get; set; } = new();
        public List<NurseViewModel> Nurses { get; set; } = new();
        public AppointmentCreateEditViewModel Appointment { get; set; } = new();
    }
}
