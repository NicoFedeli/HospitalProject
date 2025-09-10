using System.Security.Claims;
using Hospital.Models.Doctor;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
using Hospital.Models.Record;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class RecordController:Controller
    {
        private readonly IApiHelper _api;

        public RecordController(IApiHelper api)
        {
            _api = api;
        }

        // GET: /Record/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetAsync<List<RecordViewModel>>("api/Record/GetAllRecords");

            var model = new RecordsPageViewModel
            {
                Records = response?.Data ?? new List<RecordViewModel>()
            };

            return View(model);
        }


        // POST: /Record/
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Records(RecordsPageViewModel model)
        //{
        //    // Ricarico pazienti per la select
        //    var patientsResponse = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
        //    model.Patients = patientsResponse?.Data ?? new List<PatientViewModel>();

        //    if (model.SelectedPatientId <= 0)
        //    {
        //        TempData["ErrorTitle"] = "No Patient Selected";
        //        TempData["ErrorMessage"] = "Please select a valid patient.";
        //        return View(model);
        //    }

        //    // Chiamata per ottenere i record del paziente selezionato
        //    var recordsResponse = await _api.GetAsync<List<RecordViewModel>>($"api/User/GetRecordsByPatient/{model.SelectedPatientId}");
        //    model.Records = recordsResponse?.Data ?? new List<RecordViewModel>();

        //    return View(model);
        //}


        // GET: /Record/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }

            var model = new CreateRecordsViewModel();

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

        //// POST: /Record/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(RecordCreateEditViewModel record)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var fullModel = await BuildCreateViewModel(record);
        //        TempData["ErrorTitle"] = "Invalid Form";
        //        TempData["ErrorMessage"] = "Please correct the errors in the form.";
        //        return View(fullModel);
        //    }

        //    try
        //    {
        //        var response = await _api.PostAsync<RecordCreateEditViewModel>("api/Record/CreateRecord", record);

        //        if (response.Status == "OK")
        //        {
        //            TempData["SuccessTitle"] = "record created successfully!";
        //            TempData["SuccessMessage"] = $"record ID {response.Data?.ID} created.";
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            var fullModel = await BuildCreateViewModel(record);
        //            TempData["ErrorTitle"] = "Error creating record.";
        //            TempData["ErrorMessage"] = response.Message ?? "Failed to create record.";
        //            return View(fullModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var fullModel = await BuildCreateViewModel(record);
        //        TempData["ErrorTitle"] = "Unexpected error while creating the record.";
        //        TempData["ErrorMessage"] = ex.Message;
        //        return View(fullModel);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Records(RecordsPageViewModel model)
        //{
        //    // Ricarico pazienti per la select
        //    var patientsResponse = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
        //    model.Patients = patientsResponse?.Data ?? new List<PatientViewModel>();

        //    if (model.SelectedPatientId <= 0)
        //    {
        //        TempData["ErrorTitle"] = "No Patient Selected";
        //        TempData["ErrorMessage"] = "Please select a valid patient.";
        //        return View(model);
        //    }

        //    // Chiamata per ottenere i record del paziente selezionato
        //    var recordsResponse = await _api.GetAsync<List<RecordViewModel>>($"api/User/GetRecordsByPatient/{model.SelectedPatientId}");
        //    model.Records = recordsResponse?.Data ?? new List<RecordViewModel>();

        //    return View(model);
        //}

    }
}
