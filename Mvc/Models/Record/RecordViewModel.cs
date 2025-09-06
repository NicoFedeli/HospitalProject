using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Record
{
    public class RecordViewModel
    {
        [Required]
        [Display(Name ="ID Record")]
        public int ID { get; set; }

        [Required]
        [Display(Name ="ID Patient")]
        public int IDPatient { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } // Mostrato solo a schermo

        [Required]
        [Display(Name ="ID Doctor")]
        public int IDDoctor { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } // Mostrato solo a schermo

        [Required]
        [Display(Name ="ID Nurse")]
        public int IDNurse { get; set; }

        [Display(Name = "Nurse Name")]
        public string NurseName { get; set; } // Mostrato solo a schermo

        [Display(Name ="Diagnosis")]
        public string Diagnosis { get; set; }

        [Display(Name ="Prescription")]
        public string Prescription { get; set; }

        [Display(Name ="Treatment")]
        public string Treatment { get; set; }
    }
}
