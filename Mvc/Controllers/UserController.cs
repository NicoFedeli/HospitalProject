using System.Security.Claims;
using Hospital.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class UserController : Controller
    {
        private readonly IApiService _apiService;

        public UserController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // ✅ Mostra form login
        // GET: /User/Login
        [AllowAnonymous]
        //[HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Login
        // POST: /User/Login
        [HttpPost]
        //[ValidateAntiForgeryToken] da capire a che cosa può servire
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ApiResponse<UserViewModel> response = await _apiService.PostAsync<UserViewModel>("api/User/Login", model);

            if (response.Status == "OK" && response.Data != null)
            {
                //// esempio: salvi in sessione l’utente loggato
                //HttpContext.Session.SetString("UserId", response.Data.Id.ToString());

                //// Success , create cookie
                //var claims = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.Name, user.Email),
                //        new Claim("Name", user.FirstName),
                //        new Claim(ClaimTypes.Role, "User"),
                //    };

                //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                HttpContext.Session.SetString("ToastType", "success");
                HttpContext.Session.SetString("ToastMessage", "Login effettuato con successo!");
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("ToastType", "error");
            HttpContext.Session.SetString("ToastMessage", response.Message ?? "Errore login");

            //ModelState.AddModelError("", response.Message ?? "Errore login");
            return View(model);
        }

        //// ✅ Signup
        //[HttpPost]
        //public async Task<IActionResult> Signup(UserSignupViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var response = await _apiService.PostAsync<UserViewModel>("api/users/signup", model);

        //    if (response.Status == "OK")
        //        return RedirectToAction("Login");

        //    ModelState.AddModelError("", response.Message ?? "Errore registrazione");
        //    return View(model);
        //}

        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ✅ Gestione utenti (es. lista)
        public async Task<IActionResult> List()
        {
            var response = await _apiService.GetAsync<List<UserViewModel>>("api/users", new Dictionary<string, string>());
            return View(response.Data ?? new List<UserViewModel>());
        }
    }
}
