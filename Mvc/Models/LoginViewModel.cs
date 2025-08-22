using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username obbligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
