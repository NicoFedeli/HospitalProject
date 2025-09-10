using System.Security.Claims;
using Hospital.Models;
using Hospital.Models.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IApiHelper _api;

        public DoctorController(IApiHelper api)
        {
            _api = api;
        }


        // GET: /Doctor/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;  // Magari lo uso per mostrare/nascondere certe funzionalità (Edit, Delete, ecc...)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }
            

            var response = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");

            
            if(response == null || response.Data == null)
            {
                // Gestione errore, magari con un messaggio più specifico
                TempData["ErrorMessage"] = "Unable to fetch doctors at the moment. Please try again later.";
                return View(new List<DoctorViewModel>());
            }

            // Passo la lista di dottori
            return View(response.Data);
        }

        // GET: /Doctor/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;  
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");


            if (response == null || response.Data == null)
            {
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = "Unable to fetch doctors at the moment. Please try again later.";
                return View(new List<DoctorViewModel>());
            }

            var model = new DoctorEditPageViewModel
            {
                Doctors = response.Data
            };

            return View(model);
        }

        // POST: /Doctor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorEditPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ricarico la lista dottori in caso di errore
                var response = await _api.GetAsync<List<DoctorViewModel>>("api/User/GetAllDoctors");
                model.Doctors = response?.Data ?? new List<DoctorViewModel>();

                return View(model);
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "DoctorAdmin")
            {
                TempData["ErrorTitle"] = "Unauthorized";
                TempData["ErrorMessage"] = "You do not have permission to perform this action.";
                return RedirectToAction("Edit");
            }
            var updateResponse = await _api.PutAsync<DoctorEditViewModel>(
                "api/User/ModifyDoctor",
                model.DoctorToEdit
            );

            if (updateResponse.Status == "OK")
            {
                TempData["SuccessTitle"] = "Doctor updated successfully!";
                TempData["SuccessMessage"] = $"Doctor ID {model.DoctorToEdit.ID} was updated.";
                return RedirectToAction("Edit");
            }
            else
            {
                TempData["ErrorTitle"] = "Update Failed";
                TempData["ErrorMessage"] = updateResponse.Message ?? "Unable to update doctor.";
                return View(model);
            }
        }

    }
}
