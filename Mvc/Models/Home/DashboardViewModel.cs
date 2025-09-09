using Hospital.Models.Appointment;

namespace Hospital.Models.Home
{
    public class DashboardViewModel
    {
        // Per Doctor e Nurse
        public int TotalDoctors { get; set; }
        public int TotalNurses { get; set; }
        public int TotalPatients { get; set; }

        // Per Patient
        public int PaidBills { get; set; }
        public int UnpaidBills { get; set; }

        // Per tutti (lista appuntamenti)
        public List<AppointmentViewModel> Appointments { get; set; } = new();
    }
}
