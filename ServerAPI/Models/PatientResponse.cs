namespace HospitalAPI.Models
{
    public class PatientResponse
    {
        public string Status { get; set; }
        //public string Message { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
