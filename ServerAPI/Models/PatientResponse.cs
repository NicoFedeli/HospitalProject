namespace HospitalAPI.Models
{
    public class PatientResponse
    {
        public string Status { get; set; }
        public List<Patient> Data { get; set; }
    }
}
