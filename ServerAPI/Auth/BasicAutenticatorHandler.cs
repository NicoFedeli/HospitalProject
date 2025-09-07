using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace DemoApi.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider clock)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        string authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        if (!authorizationHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var token = authorizationHeader.Substring(6);
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        var credentials = credentialAsString.Split(":");
        if (credentials?.Length != 2)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var username = credentials[0];
        var password = credentials[1];


        using (var context = new HospitalDbContext())
        {
            var claims = new Claim[] { };
            try
            {
                var doctor = context.doctors.FirstOrDefault(x => x.Username == username && x.Password == password);
                if (doctor == null)
                {
                    var nurse = context.nurses.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (nurse == null)
                    {
                        var patient = context.patients.FirstOrDefault(x => x.Username == username && x.Password == password);
                        if (patient == null)
                            return AuthenticateResult.Fail("Unauthorized");
                        else
                        {
                            claims = new[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, username),
                                new Claim(ClaimTypes.Role, "Patient")
                            };
                        }
                    }
                    else if (nurse.Admin)
                    {
                        claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, username),
                            new Claim(ClaimTypes.Role, "NurseAdmin")
                        };
                    }
                    else
                    {
                        claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, username),
                            new Claim(ClaimTypes.Role, "Nurse")
                        };
                    }
                }
                else if (doctor.Admin)
                {
                    claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, username),
                        new Claim(ClaimTypes.Role, "DoctorAdmin")
                    };
                }
                else
                {
                    claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, username),
                        new Claim(ClaimTypes.Role, "Doctor")
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }




            var identity = new ClaimsIdentity(claims, "Basic");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
    }

}
