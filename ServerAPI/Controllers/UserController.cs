using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RDF"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: "IssuerServer",
              audience: "AudienceServer",
              claims: claims,
              expires: DateTime.UtcNow,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpGet("Login",Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult Get(string username, string password)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        return SearchUser(username, password, context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        return BadRequest(new GetResponse()
                        {
                            Status = "KO",
                            Message = ex.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest(new GetResponse()
                {
                    Status = "KO",
                    Message = ex.Message
                });
            }
        }

        private IActionResult SearchUser(string username, string password, HospitalDbContext context)
        {
            var doctor = context.doctors.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (doctor == null)
            {
                var nurse = context.nurses.FirstOrDefault(x => x.Username == username && x.Password == password);
                if (nurse == null)
                {
                    var patient = context.patients.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (patient == null)
                        return BadRequest(new GetResponse()
                        {
                            Status = "KO",
                            Message = $"{username} not found in DB"
                        });
                    else
                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"{username} logged succesfully as patient"
                        });
                }
                else
                    return Ok(new GetResponse()
                    {
                        Status = "OK",
                        Message = $"{username} logged succesfully as nurse"
                    });
            }
            else
                return Ok(new GetResponse()
                {
                    Status = "OK",
                    Message = $"{username} logged succesfully as doctor"
                });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddDoctor", Name = "AddDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreateDoctor(Doctor doctor)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    var newDoctor = new Doctor()
                    {
                        Name = doctor.Name,
                        Surname = doctor.Surname,
                        Username = doctor.Username,
                        Password = doctor.Password,
                        Speciality = doctor.Speciality,
                        Phone = doctor.Phone,
                        Department = doctor.Department,
                        Admin = doctor.Admin
                    };
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, newDoctor.Username);
                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Username = newDoctor.Username,
                                Message = "already exists in our database"
                            });

                        context.doctors.Add(newDoctor);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Username = newDoctor.Username,
                            Message = $"Succesfully created a new Doctor"
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        var response = new ResponsePostCreateUser()
                        {
                            Status = "KO",
                            Username = newDoctor.Username,
                            Message = ex.Message
                        };

                        return BadRequest(response);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest(new ResponsePostCreateUser()
                {
                    Status = "KO",
                    Username = doctor.Name,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNurse", Name = "AddNurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreateNurse(Nurse nurse)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    var newNurse = new Nurse()
                    {
                        Name = nurse.Name,
                        Surname = nurse.Surname,
                        Username = nurse.Username,
                        Password = nurse.Password,
                        Phone = nurse.Phone,
                        Department = nurse.Department,
                        Admin = nurse.Admin
                    };
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, newNurse.Username);

                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Username = newNurse.Username,
                                Message = "already exists in our database"
                            });

                        context.nurses.Add(newNurse);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Username = newNurse.Username,
                            Message = $"Succesfully created a new Nurse"
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        var response = new ResponsePostCreateUser()
                        {
                            Status = "KO",
                            Username = newNurse.Username,
                            Message = ex.Message
                        };

                        return BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest(new ResponsePostCreateUser()
                {
                    Status = "KO",
                    Username = nurse.Name,
                    Message = ex.Message
                });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("AddPatient", Name = "AddPatient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreatePatient(Patient patient)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    var newPatient = new Patient()
                    {
                        Name = patient.Name,
                        Surname = patient.Surname,
                        Username = patient.Username,
                        Password = patient.Password,
                        Age = patient.Age,
                        Address = patient.Address,
                        Phone = patient.Phone
                    };
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, newPatient.Username);

                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Username = newPatient.Username,
                                Message = "already exists in our database"
                            });

                        context.patients.Add(newPatient);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Username = newPatient.Username,
                            Message = $"Succesfully created a new Patient"
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        var response = new ResponsePostCreateUser()
                        {
                            Status = "KO",
                            Username = newPatient.Username,
                            Message = ex.Message
                        };

                        return BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest(new ResponsePostCreateUser()
                {
                    Status = "KO",
                    Username = patient.Name,
                    Message = ex.Message
                });
            }
        }

        private static bool UsernameAlreadyExist(HospitalDbContext context, string Username)
        {
            bool alreadyExist;
            var doctorAlresdyExist = context.doctors.FirstOrDefault(x => x.Username == Username);
            var nurseAlresdyExist = context.nurses.FirstOrDefault(x => x.Username == Username);
            var patientAlresdyExist = context.patients.FirstOrDefault(x => x.Username == Username);
            if (doctorAlresdyExist != null || nurseAlresdyExist != null || patientAlresdyExist != null)
                alreadyExist = true;
            else alreadyExist = false;

            return alreadyExist;
        }
    }
}

