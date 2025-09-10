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
    }
}
