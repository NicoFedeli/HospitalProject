using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Models.Bill
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }
    public class BillCreateEditViewModel
    {
        [Display(Name = "ID Payment")]
        public int ID { get; set; } // Non richiesto per la creazione

        [Range(1, int.MaxValue, ErrorMessage = "Please select a patient")]
        [Required(ErrorMessage = "Patient ID is required")]
        [Display(Name = "ID Patient")]
        public int IDPatient { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(PaymentStatus), ErrorMessage = "Invalid status")]
        [Display(Name = "Status")]
        public PaymentStatus Status { get; set; }

        // Dropdown support
        public List<SelectListItem> Patients { get; set; } = new List<SelectListItem>();
    }
}
