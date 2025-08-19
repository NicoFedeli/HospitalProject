namespace HospitalAPI.Models
{
    public class PatientResponse
    {
        public string Status { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
