using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hospital.Models.Constant;

namespace Hospital.Models.Doctor
{
    public class DoctorEditViewModel
    {
        [Required]
        [Key]
        public int ID { get; set; } // Not necessary for creation, but useful for editing

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Speciality is required")]
        [StringLength(50)]
        [DisplayName("Speciality")]
        [SpecialityValidation(ErrorMessage = "Invalid speciality")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20)]
        [DisplayName("Username")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        [Display(Name = "Department")]
        [DepartmentValidation(ErrorMessage = "Invalid department")]
        public string Department { get; set; }

    }
}
