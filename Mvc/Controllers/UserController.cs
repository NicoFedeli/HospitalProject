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
                HttpContext.Session.SetString("ToastMessage", $"Welcome {model.Username}!");
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("ToastType", "error");
            HttpContext.Session.SetString("ToastMessage", response.Message ?? "Login Error");

            //ModelState.AddModelError("", response.Message ?? "Errore login");
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
                // Toast di errore settato in ModelState
                HttpContext.Session.SetString("ToastType", "error");
                HttpContext.Session.SetString("ToastMessage", "Error creating user");
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
                            HttpContext.Session.SetString("ToastType", "success");
                            HttpContext.Session.SetString("ToastMessage", "Patient created! Please, Log-in");
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
                            HttpContext.Session.SetString("ToastType", "success");
                            HttpContext.Session.SetString("ToastMessage", "Nurse created! Please, Log-in");
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
                            HttpContext.Session.SetString("ToastType", "success");
                            HttpContext.Session.SetString("ToastMessage", "Doctor created! Please, Log-in");
                            return RedirectToAction("Login", "User");
                        }
                        ModelState.AddModelError("", resp.Message ?? "Error creating doctor");
                        break;
                    }
            }

            // Toast di errore settato in ModelState
            HttpContext.Session.SetString("ToastType", "error");
            HttpContext.Session.SetString("ToastMessage", "Error creating user");

            // Se arrivi qui, qualcosa non ha funzionato
            return View(vm);
        }

        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ✅ Gestione utenti (es. lista)
        public async Task<IActionResult> List()
        {
            var response = await _api.GetAsync<List<UserViewModel>>("api/users", new Dictionary<string, string>());
            return View(response.Data ?? new List<UserViewModel>());
        }
    }
}
