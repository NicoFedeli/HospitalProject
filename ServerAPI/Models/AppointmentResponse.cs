using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class AppointmentResponse
    {
        public string Status { get; set; }
        public List<ViewAppoinment> Data { get; set; }
    }

    public class ViewAppoinment
    {
        [Key]
        public int ID { get; set; }
        public string PatientName { get; set; }
        [Required]
        public int IDPatient { get; set; }
        public string DoctorName { get; set; }
        [Required]
        public int IDDoctor { get; set; }
        public string NurseName { get; set; }
        public int? IDNurse { get; set; } // l'infermiera può non servire durante l'appuntamento
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Department { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Treatment { get; set; }
    }
}
