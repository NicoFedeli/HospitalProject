namespace HospitalAPI.Models
{
    public class AppointmentResponse
    {
        public string Status { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
