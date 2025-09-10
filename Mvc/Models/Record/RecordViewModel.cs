using System.ComponentModel.DataAnnotations;
using Hospital.Models.Doctor;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;

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

    public class RecordsPageViewModel
    {
        // Lista dei record del paziente selezionato
        public List<RecordViewModel> Records { get; set; } = new();
    }

    public class CreateRecordsViewModel
    {
        // Lista pazienti per la tendina
        public List<PatientViewModel> Patients { get; set; } = new();

        //Lista dottori per la tendina
        public List<DoctorViewModel> Doctors { get; set; } = new();

        //Lista Infermieri per la tendina
        public List<NurseViewModel> Nurses { get; set; } = new();

        // Record del paziente selezionato
        public RecordCreateEditViewModel Record { get; set; }
    }

}
