using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Nurse
{
    public class NurseCreateEditViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Is Primary Nurse?")] // Traduzione da migliorare
        public bool Admin { get; set; }
    }
}
