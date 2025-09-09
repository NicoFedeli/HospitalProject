using System.Diagnostics;
using System.Security.Claims;
using Hospital.Models;
using Hospital.Models.Appointment;
using Hospital.Models.Bill;
using Hospital.Models.Doctor;
using Hospital.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiHelper _api;

        public HomeController(IApiHelper api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }

            var model = new DashboardViewModel();

            // Logica in base al ruolo
            switch (role)
            {
                case "Doctor":
                case "DoctorAdmin":
                case "Nurse":
                case "NurseAdmin":
                    // Totale dottori nel dipartimento
                    ApiResponse<List<DoctorViewModel>> doctors = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDepartmentDoctors", new { doctorId = userId});
                    model.TotalDoctors = doctors.Data?.Count ?? 0;

                    // Totale infermieri nel dipartimento
                    var nurses = await _api.GetAsync<List<UserViewModel>>("api/User/GetAllDepartmentNurseFromDoctor", new { doctorId = userId });
                    model.TotalNurses = nurses.Data?.Count ?? 0;

                    // Appuntamenti del dottore
                    var appointments = await _api.GetAsync<List<AppointmentViewModel>>("api/appointment/GetAllDoctorAppointments", new {doctorId = userId});
                    model.Appointments = appointments.Data ?? new List<AppointmentViewModel>();
                    break;

                case "Patient":
                    // Qui lo userId equivale all'id di un paziente

                    // Bills pagati
                    var paidBills = await _api.GetAsync<List<BillViewModel>>("api/Bill/GetPaidPatientBills", new {id=userId});
                    model.PaidBills = paidBills.Data?.Count ?? 0;

                    // Bills non pagati
                    var unpaidBills = await _api.GetAsync<List<BillViewModel>>("api/Bill/GetNotPaidPatientBills", new {id=userId});
                    model.UnpaidBills = unpaidBills.Data?.Count ?? 0;

                    // Appuntamenti del paziente
                    var patientAppointments = await _api.GetAsync<List<AppointmentViewModel>>("api/Appointment/GetAllPatientAppointments", new {patientId=userId});
                    model.Appointments = patientAppointments.Data ?? new List<AppointmentViewModel>();
                    break;
            }

            return View(model);
        }


        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
