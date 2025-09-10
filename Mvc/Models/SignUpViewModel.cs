using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hospital.Models.Constant;

namespace Hospital.Models
{
    public enum UserRole { Patient, Nurse, Doctor }
    


    // Campi comuni per tutti i ruoli
    public class SignUpCommonViewModel
    {
        [DisplayName("First Name")]
        [Required, StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Last Name")]
        [Required, StringLength(50)]
        public string Surname { get; set; }

        [DisplayName("Username")]
        [Required, StringLength(50)]
        public string Username { get; set; }

        [DisplayName("Password")]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required, Compare(nameof(Password))]
        [StringLength(100, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Choose a Role")]
        [Required]
        public UserRole Role { get; set; }
    }

    // Prendo i campi restanti dalle rispettive CreateEditViewModel
    public class PatientCreateViewModel
    {
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

    public class NurseCreateViewModel
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        [Display(Name = "Department")]
        [DepartmentValidation(ErrorMessage = "Invalid department")]
        public string Department { get; set; }

        // Lascio di default false, mostro comuqnque la checkbox in view per mostrare nel caso l'admin la voglia settare 
        // In produzione, solo un admin può settare un altro admin
        [DisplayName("Is Primary Nurse?")]
        public bool Admin { get; set; } = false;
    }

    public class DoctorCreateViewModel
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        [Display(Name = "Department")]
        [DepartmentValidation(ErrorMessage = "Invalid department")]
        public string Department { get; set; }


        [Required(ErrorMessage = "Speciality is required")]
        [StringLength(50)]
        [DisplayName("Speciality")]
        [SpecialityValidation(ErrorMessage = "Invalid speciality")]
        public string Speciality { get; set; }


        // Lascio di default false, mostro comuqnque la checkbox in view per mostrare nel caso l'admin la voglia settare 
        // In produzione, solo un admin può settare un altro admin
        [DisplayName("Is Primary Doctor?")]
        public bool Admin { get; set; } = false; 
    }


    public class SignUpViewModel
    {
        public SignUpCommonViewModel Common { get; set; } = new();
        public PatientCreateViewModel? Patient { get; set; } = new();
        public NurseCreateViewModel? Nurse { get; set; } = new();
        public DoctorCreateViewModel? Doctor { get; set; } = new();
    }
}
