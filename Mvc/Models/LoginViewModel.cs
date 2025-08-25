using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username matadory")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password matadory")]
        [StringLength(100, MinimumLength = 5, ErrorMessage ="min 5 character allowed")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
