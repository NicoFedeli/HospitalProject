using System.Security.Claims;
using Hospital.Models.Appointment;
using Hospital.Models.Doctor;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class AppointmentController: Controller
    {
        private readonly IApiHelper _api;

        public AppointmentController(IApiHelper api)
        {
            _api = api;
        }

        // GET: /Appointment/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }

            var model = new CreateAppointmentViewModel();

            try
            {
                // Recupero i dottori
                var doctorsResponse = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
                model.Doctors = doctorsResponse.Status == "OK" ? doctorsResponse.Data ?? new List<DoctorViewModel>() : new List<DoctorViewModel>();

                // Recupero le infermiere
                var nursesResponse = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
                model.Nurses = nursesResponse.Status == "OK" ? nursesResponse.Data ?? new List<NurseViewModel>() : new List<NurseViewModel>();

                // Recupero i pazienti
                var patientsResponse = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
                model.Patients = patientsResponse.Status == "OK" ? patientsResponse.Data ?? new List<PatientViewModel>() : new List<PatientViewModel>();
            }
            catch (Exception ex)
            {
                // In caso di errore imprevisto, mostro un messaggio
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine($"[AppointmentController.Create] Errore: {ex.Message}");

                // Evito crash e passo un modello vuoto
                return View();
            }


            return View(model);
        }


        // POST: /Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateEditViewModel appointment)
        {
            if (!ModelState.IsValid)
            {
                var fullModel = await BuildCreateViewModel(appointment);
                TempData["ErrorTitle"] = "Invalid Form";
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(fullModel);
            }

            try
            {
                var response = await _api.PostAsync<AppointmentCreateEditViewModel>("api/Appointment/CreateAppointment", appointment);

                if (response.Status == "OK")
                {
                    TempData["SuccessTitle"] = "Appointment created successfully!";
                    TempData["SuccessMessage"] = $"Appointment ID {response.Data?.ID} created.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var fullModel = await BuildCreateViewModel(appointment);
                    TempData["ErrorTitle"] = "Error creating appointment.";
                    TempData["ErrorMessage"] = response.Message ?? "Failed to create appointment.";
                    return View(fullModel);
                }
            }
            catch (Exception ex)
            {
                var fullModel = await BuildCreateViewModel(appointment);
                TempData["ErrorTitle"] = "Unexpected error while creating the appointment.";
                TempData["ErrorMessage"] = ex.Message;
                return View(fullModel);
            }
        }


        // Creo la viewModel per ritornare alla GET (alla pagina mostrata)
        private async Task<CreateAppointmentViewModel> BuildCreateViewModel(AppointmentCreateEditViewModel appointment)
        {
            var model = new CreateAppointmentViewModel
            {
                Appointment = appointment
            };

            var doctors = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
            model.Doctors = doctors.Data ?? new List<DoctorViewModel>();

            var nurses = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
            model.Nurses = nurses.Data ?? new List<NurseViewModel>();

            var patients = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
            model.Patients = patients.Data ?? new List<PatientViewModel>();

            return model;
        }

    }
}
