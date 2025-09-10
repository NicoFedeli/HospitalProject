using System.Security.Claims;
using Hospital.Models.Nurse;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class NurseController : Controller
    {
        private readonly IApiHelper _api;

        public NurseController(IApiHelper api)
        {
            _api = api;
        }

        // GET: /Nurse/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;  // Magari lo uso per mostrare/nascondere certe funzionalità (Edit, Delete, ecc...)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");


            if (response == null || response.Data == null)
            {
                // Gestione errore, magari con un messaggio più specifico
                TempData["ErrorMessage"] = "Unable to fetch nurses at the moment. Please try again later.";
                return View(new List<NurseViewModel>());
            }

            // Passo la lista di dottori
            return View(response.Data);
        }


        // GET: /Nurse/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }

            var response = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");

            var model = new NurseEditPageViewModel();

            if (response == null || response.Data == null)
            {
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = "Unable to fetch nurses at the moment. Please try again later.";
                model.Nurses = new List<NurseViewModel>(); // Provide an empty list to avoid null reference
            }
            else
            {
                model.Nurses = response.Data;
            }

            return View(model);
        }

        // POST: /Nurse/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NurseEditPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ricarico la lista dottori in caso di errore
                var response = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");
                model.Nurses = response?.Data ?? new List<NurseViewModel>();

                return View(model);
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "NurseAdmin")
            {
                TempData["InfoTitle"] = "Unauthorized";
                TempData["InfoMessage"] = "You do not have permission to perform this action.";
                return RedirectToAction("Edit");
            }
            var updateResponse = await _api.PutAsync<NurseEditViewModel>(
                "api/User/ModifyNurse",
                model.NurseToEdit
            );

            if (updateResponse.Status == "OK")
            {
                TempData["SuccessTitle"] = "Nurse updated successfully!";
                TempData["SuccessMessage"] = $"Nurse ID {model.NurseToEdit.ID} was updated.";
                return RedirectToAction("Edit");
            }
            else
            {
                TempData["ErrorTitle"] = "Update Failed";
                TempData["ErrorMessage"] = updateResponse.Message ?? "Unable to update nurse.";
                return View(model);
            }
        }


        // GET: /Nurse/Delete
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn", "User");
            }


            var response = await _api.GetAsync<List<NurseViewModel>>("api/User/GetAllNurses");

            var model = new NurseDeletePageViewModel();

            if (response == null || response.Data == null)
            {
                TempData["ErrorTitle"] = "Error during data fetching.";
                TempData["ErrorMessage"] = "Unable to fetch nurses at the moment. Please try again later.";
                model.Nurses = new List<NurseViewModel>(); // Provide an empty list to avoid null reference
            }
            else
            {
                model.Nurses = response.Data;
            }

            return View(model);
        }

        // POST: /Nurse/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(NurseDeletePageViewModel model)
        {
            if (model.NurseToDelete == null || model.NurseToDelete.ID <= 0)
            {
                TempData["ErrorTitle"] = "Invalid Nurse";
                TempData["ErrorMessage"] = "Please select a valid nurse to delete.";
                return RedirectToAction("Delete");
            }

            // Controllo permessi
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "NurseAdmin")
            {
                TempData["ErrorTitle"] = "Unauthorized";
                TempData["ErrorMessage"] = "You do not have permission to perform this action.";
                return RedirectToAction("Delete");
            }

            // Eseguo la DELETE sull'API
            var response = await _api.DeleteAsync<object>($"api/User/DeleteNurse?nurseId={model.NurseToDelete.ID}");

            if (response.Status == "OK")
            {
                TempData["SuccessTitle"] = "Nurse Deleted!";
                TempData["SuccessMessage"] = $"Nurse ID {model.NurseToDelete.ID} was successfully removed.";
            }
            else
            {
                TempData["ErrorTitle"] = "Delete Failed";
                TempData["ErrorMessage"] = response.Message ?? "Unable to delete nurse at this time.";
            }

            return RedirectToAction("Delete");
        }


    }
}
