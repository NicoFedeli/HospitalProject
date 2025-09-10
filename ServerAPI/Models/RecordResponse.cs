using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class RecordResponse
    {
        public string Status { get; set; }
        public List<ViewRecord> Data { get; set; }
    }

    public class ViewRecord
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public int IDPatient { get; set; }
        public string PatientName { get; set; } // Mostrato solo a schermo
        [Required]
        public int IDDoctor { get; set; }
        public string DoctorName { get; set; } // Mostrato solo a schermo

        [Required]
        public int IDNurse { get; set; }
        public string NurseName { get; set; } // Mostrato solo a schermo
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Treatment { get; set; }
    }
}