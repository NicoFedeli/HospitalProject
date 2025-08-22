using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IConfiguration configuration)
        {
            string baseUrl = configuration["ApiSettings:BaseUrl"]; // Inietto IConfiguration per leggere le impostazioni dal file appsettings.json
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl)};
        }

        //📂 Struttura tipica che ti consiglio
        //Controllers/
        //    UserController.cs         // login, signup, logout, gestione utenti
        //    DoctorController.cs       // azioni su Doctor (lista, dettagli, creazione, modifica)
        //    NurseController.cs        // idem per Nurse
        //    PatientController.cs      // idem per Patient
        //    AppointmentController.cs  // idem per Appointment
        //    BillController.cs         // idem per Bill
        //    RecordController.cs       // idem per Record


        public IActionResult Index()
        {
            List
            return View();
        }
    }
}
