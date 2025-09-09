using System.ComponentModel;
using Hospital.Models.Enums;

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
        public Speciality? Speciality { get; set; }

        [DisplayName("Username")]
        public string Username { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Department")]
        public Department? Department { get; set; }

        [DisplayName("Is Primary Doctor?")]
        public bool Admin { get; set; }
    }


    public class DoctorDataWrapper
    {
        public List<DoctorViewModel> Doctors { get; set; } = new();
    }
}
