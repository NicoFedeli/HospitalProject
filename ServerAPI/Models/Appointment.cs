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
        [Required]
        public int IDNurse {get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Department { get; set; }

    }
}
