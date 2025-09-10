using System.Security.Claims;
using Hospital.Models;
using Hospital.Models.Bill;
using Hospital.Models.Patient;
using Hospital.Models.Patient;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class PatientController : Controller
    {
        private readonly IApiHelper _api;

        public PatientController(IApiHelper api)
        {
            _api = api;
        }


        // GET: /Patient/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;  // Magari lo uso per mostrare/nascondere certe funzionalità (Edit, Delete, ecc...)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");


            if (response == null || response.Data == null)
            {
                // Gestione errore, magari con un messaggio più specifico
                TempData["ErrorMessage"] = "Unable to fetch patients at the moment. Please try again later.";
                return View(new List<PatientViewModel>());
            }

            // Passo la lista di dottori
            return View(response.Data);
        }


        // GET: /Patient/PaidBills
        [HttpGet]
        public async Task<IActionResult> PaidBills()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }
            // Solo i pazienti possono vedere le loro fatture
            if (role != "Patient")
            {
                return Forbid();
            }
            var response = await _api.GetAsync<List<BillViewModel>>("api/Bill/GetPaidPatientBills", new { id = userId });
            if (response == null || response.Data == null)
            {
                TempData["ErrorMessage"] = "Unable to fetch bills at the moment. Please try again later.";
                return View(new List<BillViewModel>());
            }
            return View(response.Data);
        }


        // GET: /Bill/UnPaidBills
        [HttpGet]
        public async Task<IActionResult> UnPaidBills()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<BillViewModel>>("api/Bill/GetNotPaidPatientBills", new { id = userId });


            if (response == null || response.Data == null)
            {
                // Gestione errore, magari con un messaggio più specifico
                TempData["ErrorMessage"] = "Unable to fetch unpaid bills at the moment. Please try again later.";
                return View(new List<BillViewModel>());
            }

            // Passo la lista di dottori
            return View(response.Data);
        }

        // PATCH: /Patient/PayBill
        [HttpGet]
        public async Task<IActionResult> PayBill(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }
            var patchData = new { Status = "PAID" };

            var response = await _api.PatchAsync<ApiResponse<Bill>>($"api/Bill/PayBill?id={id}&patientId={userId}", patchData);

            if (response.Status == "OK")
            {
                TempData["SuccessTitle"] = "Bill successfully paid!";
                TempData["SuccessMessage"] = "You heve just paid the bill!";
                return RedirectToAction("UnPaidBills");
            }

            TempData["ErrorTitle"] = "Error paying bill.";
            TempData["ErrorMessage"] = "Something went wrong, please try again.";
            return RedirectToAction("UnPaidBills");
        }


        // GET: /Patient/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }

            var response = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");

            var model = new PatientEditPageViewModel();

            if (response == null || response.Data == null)
            {
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = "Unable to fetch nurses at the moment. Please try again later.";
                model.Patients = new List<PatientViewModel>(); // Provide an empty list to avoid null reference
            }
            else
            {
                model.Patients = response.Data;
            }

            return View(model);
        }

        // POST: /Patient/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PatientEditPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ricarico la lista dottori in caso di errore
                var response = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");
                model.Patients = response?.Data ?? new List<PatientViewModel>();

                return View(model);
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "PatientAdmin" && role != "Patient" && role != "NurseAdmin" && role != "Nurse")
            {
                TempData["InfoTitle"] = "Unauthorized";
                TempData["InfoMessage"] = "You do not have permission to perform this action.";
                return RedirectToAction("Edit");
            }
            var updateResponse = await _api.PutAsync<PatientEditViewModel>(
                "api/User/ModifyPatient",
                model.PatientToEdit
            );

            if (updateResponse.Status == "OK")
            {
                TempData["SuccessTitle"] = "Patient updated successfully!";
                TempData["SuccessMessage"] = $"Patient ID {model.PatientToEdit.ID} was updated.";
                return RedirectToAction("Edit");
            }
            else
            {
                TempData["ErrorTitle"] = "Update Failed";
                TempData["ErrorMessage"] = updateResponse.Message ?? "Unable to update patient.";
                return View(model);
            }
        }


        // GET: /Patient/Delete
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<PatientViewModel>>("api/User/GetAllPatients");

            var model = new PatientDeletePageViewModel();

            if (response == null || response.Data == null)
            {
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = "Unable to fetch nurses at the moment. Please try again later.";
                model.Patients = new List<PatientViewModel>(); // Provide an empty list to avoid null reference
            }
            else
            {
                model.Patients = response.Data;
            }

            return View(model);
        }

        // POST: /Patient/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PatientDeletePageViewModel model)
        {
            if (model.PatientToDelete == null || model.PatientToDelete.ID <= 0)
            {
                TempData["ErrorTitle"] = "Invalid Patient";
                TempData["ErrorMessage"] = "Please select a valid doctor to delete.";
                return RedirectToAction("Delete");
            }

            // Controllo permessi
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "PatientAdmin")
            {
                TempData["ErrorTitle"] = "Unauthorized";
                TempData["ErrorMessage"] = "You do not have permission to perform this action.";
                return RedirectToAction("Delete");
            }

            // Eseguo la DELETE sull'API
            var response = await _api.DeleteAsync<object>($"api/User/DeletePatient?patientId={model.PatientToDelete.ID}");

            if (response.Status == "OK")
            {
                TempData["SuccessTitle"] = "Patient Deleted!";
                TempData["SuccessMessage"] = $"Patient ID {model.PatientToDelete.ID} was successfully removed.";
            }
            else
            {
                TempData["ErrorTitle"] = "Delete Failed";
                TempData["ErrorMessage"] = response.Message ?? "Unable to delete patient at this time.";
            }

            return RedirectToAction("Delete");
        }
    }
}
