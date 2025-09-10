using System.ComponentModel;

namespace Hospital.Models.Doctor
{
    public class DoctorViewModel
    {
        public int ID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        public string Surname { get; set; }

        [DisplayName("Specitlity")]
        public string? Speciality { get; set; }

        [DisplayName("Username")]
        public string Username { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Department")]
        public string? Department { get; set; }
    }


    public class DoctorDataWrapper
    {
        public List<DoctorViewModel> Doctors { get; set; } = new();
    }

    public class DoctorEditPageViewModel
    {
        // Lista dei dottori per la tendina
        public List<DoctorViewModel> Doctors { get; set; } = new();

        // Doctor selezionato e modificabile
        public DoctorCreateViewModel DoctorToEdit { get; set; } = new();
    }

}
