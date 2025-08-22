using System.ComponentModel;

namespace Hospital.Models.Patient
{
    public class PatientViewModel
    {
        public int ID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        public string Surname { get; set; }

        [DisplayName("Usernae")]
        public string Username { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Local Address")]
        public string Address { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }
    }
}
