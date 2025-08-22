using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Bill
{
    public class BillViewModel
    {
        [Required]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "ID Patient")]
        public int IDPatient { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } // Mostrato solo a schermo

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
