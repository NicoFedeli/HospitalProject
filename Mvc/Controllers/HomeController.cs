using System.Diagnostics;
using System.Security.Claims;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Quando avrò le api per leggere il numero di dottori, infermieri
        //public async Task<IActionResult> Index()
        //{
        //    // 1. Recupero dati utente dai claims
        //    var userRole = User.FindFirstValue(ClaimTypes.Role);
        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //    // Qui supponiamo che DepartmentId sia collegato all'utente
        //    int departmentId = 3; // ?? Da recuperare dinamicamente

        //    // 2. Chiamata API
        //    var response = await _api.GetAsync<StaffStatsViewModel>(
        //        $"api/Dashboard/GetStaffStats?departmentId={departmentId}");

        //    // 3. Passo alla View
        //    return View(response.Data);
        //}
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
