using DemoApi.Auth;
using HospitalAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge la possibilità di creare classi controller

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//RDF NEW
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<UserService>();
//IMPORTANTE  BasicAuthenticationHandler
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformationService>();


// Sessione
builder.Services.AddDistributedMemoryCache(); // necessaria per gestire lo storage in memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // durata sessione
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



//--------------------------------

var app = builder.Build();
app.UseAuthentication(); // abilita l'uso dell'autenticazione
// Configure the HTTP request pipeline. creao un pagine web per il test delle api
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // forza l'uso di https
app.UseSession(); // abilita l'uso della sessione
app.UseAuthorization(); // abilita l'uso dell'autorizzazione

// Aggiunge la possibilità di creare classi controller
app.MapControllers();

app.Run();
