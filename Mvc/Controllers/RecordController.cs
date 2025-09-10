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


        // POST: /Record/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RecordCreateEditViewModel editRecord)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorTitle"] = "Validation Error";
                TempData["ErrorMessage"] = "Invalid form data.";
                return View(editRecord);
            }

            // A questo punto puoi mostrare la pagina con i campi modificabili
            return View(editRecord);
        }


        // POST: /Record/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRecord(RecordCreateEditViewModel editRecord)
        {
            if (!ModelState.IsValid)
            {
                // Se ci sono errori, il form viene ricaricato con EditedRecord
                // e Record rimane invariato per riferimento
                TempData["ErrorTitle"] = "Validation Error";
                TempData["ErrorMessage"] = "Invalid form data.";
                return RedirectToAction("Index", "Record");
            }

            // Chiamata API o DB per salvare EditedRecord
            var response = _api.PutAsync<RecordViewModel>("api/Record/ModifyRecord", editRecord).Result;

            if (response.Status == "OK")
            {
                TempData["SuccessTitle"] = "Record updated successfully!";
                TempData["SuccessMessage"] = $"Record ID {editRecord.ID} updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["ErrorTitle"] = "Error updating record.";
            TempData["ErrorMessage"] = response.Message ?? "Failed to update record.";
            return RedirectToAction("Index", "Record");
        }


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

            // Popolo i pazienti
            var patientsResp = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
            model.Patients = patientsResp?.Data ?? new List<PatientViewModel>();

            // Popolo i dottori
            var doctorsResp = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
            model.Doctors = doctorsResp?.Data ?? new List<DoctorViewModel>();

            // Popolo gli infermieri
            var nursesResp = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
            model.Nurses = nursesResp?.Data ?? new List<NurseViewModel>();

            // Record vuoto pronto per il form
            model.Record = new RecordCreateEditViewModel();

            return View(model);
        }


        // POST: /Record/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecordsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ricarico le liste per ricreare i select
                var patientsResp = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
                model.Patients = patientsResp?.Data ?? new List<PatientViewModel>();

                var doctorsResp = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
                model.Doctors = doctorsResp?.Data ?? new List<DoctorViewModel>();

                var nursesResp = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
                model.Nurses = nursesResp?.Data ?? new List<NurseViewModel>();

                return View(model);
            }

            // Chiamo API per creare il record
            var response = await _api.PostAsync<RecordCreateEditViewModel>("api/Record/CreateRecord", model.Record);

            if (response.Status == "OK")
            {
                TempData["SuccessTitle"] = "Record created successfully!";
                TempData["SuccessMessage"] = $"Record ID {response.Data?.ID} created.";
                return RedirectToAction("Index", "Record");
            }
            else
            {
                TempData["ErrorTitle"] = "Error creating record";
                TempData["ErrorMessage"] = response.Message ?? "Failed to create record.";

                // Ricarico le liste in caso di errore
                var patientsResp = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
                model.Patients = patientsResp?.Data ?? new List<PatientViewModel>();

                var doctorsResp = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
                model.Doctors = doctorsResp?.Data ?? new List<DoctorViewModel>();

                var nursesResp = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
                model.Nurses = nursesResp?.Data ?? new List<NurseViewModel>();

                return View(model);
            }
        }

    }
}
