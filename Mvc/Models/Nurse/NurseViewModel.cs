using System.ComponentModel.DataAnnotations;
using Hospital.Models.Doctor;

namespace Hospital.Models.Nurse
{
    public class NurseViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Phnoe")]
        public string Phone { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }
    }

    public class NurseEditPageViewModel
    {
        // Lista dei dottori per la tendina
        public List<NurseViewModel> Nurses { get; set; } = new();

        // Doctor selezionato e modificabile
        public NurseEditViewModel NurseToEdit { get; set; } = new();
    }

    public class NurseDeletePageViewModel
    {
        // Lista dei dottori per la tendina
        public List<NurseViewModel> Nurses { get; set; } = new();

        // Doctor selezionato e modificabile
        public NurseEditViewModel NurseToDelete { get; set; } = new();
    }
}
