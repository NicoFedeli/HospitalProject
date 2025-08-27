using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Per iniettare IHttpContextAccessor in UI/_ToastScripts
builder.Services.AddDistributedMemoryCache(); // Memoria temporanea per sessione
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durata sessione
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Necessario per GDPR
});

// Registro HttpClient + ApiService
builder.Services.AddHttpClient<IApiHelper, ApiHelper>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

// Cookie auth (login/logout lato MVC)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/LogIn";
        o.LogoutPath = "/User/LogOut";
        o.AccessDeniedPath = "/User/AccessDenied";
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // per usare HttpContext.Session
app.UseAuthentication(); // Autenticazione
app.UseAuthorization(); // Autorizzazione

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();