using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("Record")]

    public class Record
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int IDPatient { get; set; }
        [Required]
        public int IDDoctor { get; set; }
        [Required]
        public int IDNurse { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Treatment { get; set; }
    }
}
