using System.Security.Claims;
using Hospital.Models.Patient;
using Hospital.Models.Bill;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;

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
                TempData["ErrorMessage"] = "Unable to fetch doctors at the moment. Please try again later.";
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

    }
}
