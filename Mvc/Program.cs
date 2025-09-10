using Hospital.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Tutti i controller richiedono autenticazione di default, rende [Authorize] implicito in tutti i controller
    // Nelle View di Login e SignUp uso [AllowAnonymous] per permettere l'accesso anonimo
    // In questo modo uno non loggato viene reindirizzato alla pagina di login
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

builder.Services.AddHttpContextAccessor(); // Per iniettare IHttpContextAccessor in UI/_ToastScripts
//builder.Services.AddDistributedMemoryCache(); // Memoria temporanea per sessione
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durata sessione
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true; // Necessario per GDPR
//});

builder.Services.AddTransient<BearerTokenHandler>(); // aggiundo il BearerTokenHandler come servizio transient (una nuova istanza per ogni richiesta)
builder.Services.AddHttpClient<IApiHelper, ApiHelper>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
})
.AddHttpMessageHandler<BearerTokenHandler>();


// Cookie auth (login/logout lato MVC)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/LogIn";
        o.LogoutPath = "/User/LogOut";
        o.AccessDeniedPath = "/User/AccessDenied";
        o.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Indica il tempo di validità del cookie di autenticazione se il cookie è persistente (IsPersistent=true)
        o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; //altrimenti in localhost senza HTTPS il cookie non viene settato
        //o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Solo in produzione con HTTPS
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
//app.UseSession(); // per usare HttpContext
app.UseAuthentication(); // Autenticazione
app.UseAuthorization(); // Autorizzazione

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();