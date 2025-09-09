using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("Appointment")]

    public class Appointment
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int IDPatient { get; set; }
        [Required]
        public int IDDoctor { get; set; }
        public int? IDNurse {get; set; } // l'infermiera può non servire durante l'appuntamento
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Department { get; set; }

    }
}
