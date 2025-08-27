using System.Reflection;
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
        private readonly IApiHelper _api;

        public UserController(IApiHelper api)
        {
            _api = api;
        }

        // ✅ Mostra form login
        // GET: /User/Login
        [AllowAnonymous]
        [HttpGet]
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

            ApiResponse<UserViewModel> response = await _api.PostAsync<UserViewModel>("api/User/Login", model);

            if (response.Status == "OK" && response.Data != null)
            {
                // Creo una Cookie Auth con Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.Id.ToString()),
                    new Claim(ClaimTypes.Name, response.Data.Username),
                    new Claim(ClaimTypes.Role, response.Data.Role) // es: Doctor, Nurse, Patient
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                TempData["SuccessTitle"] = "Login successful";
                TempData["SuccessMessage"] = $"Welcome back, {model.Username}!";
                return RedirectToAction("Index", "Home");
            }
            TempData["ErrorTitle"] = "Login failed";
            TempData["ErrorMessage"] = response.Message ?? "Login Error";
            return View(model);
        }

        // ✅ Mostra form SignUp
        // GET: /User/SignUp
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(); 
        }

        // ✅ Signup
        // POST: /User/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken] // Genera un token (___RequestVerificationToken) automaticamente in ogni form protetto da esso. Protegge da CSRF
        public async Task<IActionResult> SignUp(SignUpViewModel vm)
        {
            // 1) Svuota gli errori raccolti automaticamente dal binder
            ModelState.Clear();

            // 2) Valida SOLO i campi comuni
            TryValidateModel(vm.Common, prefix: "Common");

            // 3) Valida SOLO il sotto-modello in base al ruolo scelto
            switch (vm.Common?.Role)
            {
                case UserRole.Patient:
                    TryValidateModel(vm.Patient, prefix: "Patient");
                    break;

                case UserRole.Nurse:
                    TryValidateModel(vm.Nurse, prefix: "Nurse");
                    break;

                case UserRole.Doctor:
                    TryValidateModel(vm.Doctor, prefix: "Doctor");
                    break;

                default:
                    ModelState.AddModelError("Common.Role", "Ruolo non valido o mancante.");
                    break;
            }

            // 4) Se qualcosa non va, torna alla View con gli errori corretti
            if (!ModelState.IsValid)
            {
                // Toast di errore 
                TempData["ErrorTitle"] = "Signup failed";
                TempData["ErrorMessage"] = "An error occured while creating your account. Please, try again";
                return View(vm);
            }



            // Mappa in base al ruolo e chiama il backend
            switch (vm.Common.Role)
            {
                case UserRole.Patient:
                    {
                        // Validazione campi Patient
                        if (!TryValidateModel(vm.Patient, nameof(vm.Patient)))
                            return View(vm);

                        var dto = new
                        {
                            Name = vm.Common.Name,
                            Surname = vm.Common.Surname,
                            Username = vm.Common.Username,
                            Password = vm.Common.Password,
                            Age = vm.Patient.Age,
                            Phone = vm.Patient.Phone,
                            Address = vm.Patient.Address
                        };

                        var resp = await _api.PostAsync<ApiResponse<object>>("api/User/AddPatient", dto);
                        if (resp.Status == "OK")
                        {
                            TempData["SuccessTitle"] = "Account created";
                            TempData["SuccessMessage"] = "Your account has been successfully created as a Patient. You can now log in.";
                            return RedirectToAction("Login", "User");
                        }
                        ModelState.AddModelError("", resp.Message ?? "Error creating patient");
                        break;
                    }

                case UserRole.Nurse:
                    {
                        // Validazione campi Nurse
                        if (!TryValidateModel(vm.Nurse, nameof(vm.Nurse)))
                            return View(vm);

                        var dto = new
                        {
                            Name = vm.Common.Name,
                            Surname = vm.Common.Surname,
                            Username = vm.Common.Username,
                            Password = vm.Common.Password,
                            Phone = vm.Nurse.Phone,
                            Department = vm.Nurse.Department,
                            Admin = vm.Nurse.Admin // in prod da mettere sempre false, solo un admin può creare un altro admin
                        };

                        var resp = await _api.PostAsync<ApiResponse<object>>("api/User/AddNurse", dto);
                        if (resp.Status == "OK")
                        {
                            TempData["SuccessTitle"] = "Account created";
                            TempData["SuccessMessage"] = "Your account has been successfully created as a Nurse. You can now log in.";
                            return RedirectToAction("Login", "User");
                        }
                        ModelState.AddModelError("", resp.Message ?? "Error creating nurse");
                        break;
                    }

                case UserRole.Doctor:
                    {
                        // Validazione campi Doctor
                        if (!TryValidateModel(vm.Doctor, nameof(vm.Doctor)))
                            return View(vm);

                        var dto = new
                        {
                            Name = vm.Common.Name,
                            Surname = vm.Common.Surname,
                            Username = vm.Common.Username,
                            Password = vm.Common.Password,
                            Phone = vm.Doctor.Phone,
                            Department = vm.Doctor.Department,
                            Speciality = vm.Doctor.Speciality,
                            Admin = vm.Doctor.Admin // in prod da mettere sempre false, solo un admin può creare un altro admin
                        };

                        var resp = await _api.PostAsync<ApiResponse<object>>("api/User/AddDoctor", dto);
                        if (resp.Status == "OK")
                        {
                            TempData["SuccessTitle"] = "Account created";
                            TempData["SuccessMessage"] = "Your account has been successfully created as a Doctor. You can now log in.";
                            return RedirectToAction("Login", "User");
                        }
                        ModelState.AddModelError("", resp.Message ?? "Error creating doctor");
                        break;
                    }
            }

            // Toast di errore 
            TempData["ErrorTitle"] = "Signup failed";
            TempData["ErrorMessage"] = "Error, something went wrong...";

            // Se arrivi qui, qualcosa non ha funzionato
            return View(vm);
        }

        // ✅ Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            // 1. Logout lato ServerApi
            await _api.PostAsync<object>("api/User/Logout", null);

            // 2. Logout lato MVC (cookie di autenticazione)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //// 3. Pulisci sessione TempData / toastr
            //HttpContext.Session.Clear();

            TempData["SuccessTitle"] = "Bye Bye!";
            TempData["SuccessMessage"] = "Successfully logged out";

            return RedirectToAction("Index", "Home");
        }

        // ✅ Gestione utenti (es. lista)
        public async Task<IActionResult> List()
        {
            var response = await _api.GetAsync<List<UserViewModel>>("api/users", new Dictionary<string, string>());
            return View(response.Data ?? new List<UserViewModel>());
        }
    }
}
