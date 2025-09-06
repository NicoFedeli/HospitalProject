using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Models.Record
{
    public class RecordCreateEditViewModel
    {
        [Display(Name = "ID Record")]
        public int ID { get; set; } // Non richiesto per la creazione

        [Required(ErrorMessage = "Patient ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid patient")]
        [Display(Name = "ID Patient")]
        public int IDPatient { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor")]
        [Display(Name = "ID Doctor")]
        public int IDDoctor { get; set; }

        [Required(ErrorMessage = "Nurse ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid nurse")]
        [Display(Name = "ID Nurse")]
        public int IDNurse { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis must be max 500 characters")]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        [StringLength(500, ErrorMessage = "Prescription must be max 500 characters")]
        [Display(Name = "Prescription")]
        public string Prescription { get; set; }

        [StringLength(500, ErrorMessage = "Treatment must be max 500 characters")]
        [Display(Name = "Treatment")]
        public string Treatment { get; set; }

        // Dropdown support
        public List<SelectListItem> Patients { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Doctors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Nurses { get; set; } = new List<SelectListItem>();

    }
}
