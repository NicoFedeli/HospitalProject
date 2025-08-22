using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Patient
{
    public class PatientCreateEditViewModel
    {
    
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(0, 99, ErrorMessage = "Age must be between 0 and 99")]
        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Local Address")]
        public string Address { get; set; }

        [DisplayName("Phone")]
        [Phone]
        public string Phone { get; set; }
    }
}
