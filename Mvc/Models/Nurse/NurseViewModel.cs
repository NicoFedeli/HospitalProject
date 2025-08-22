using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Phnoe")]
        public string Phone { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Is Primary Nurse?")] // Traduzione da migliorare
        public bool Admin { get; set; }
    }
}
