using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        private string GenerateJwtToken(int id,string username,string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };
        
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("una-super-chiave-lunghissima-e-segreta-123456789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: "IssuerServer",
              audience: "AudienceServer",
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(60), // 60 minuti
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost("Login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetResponse()
                {
                    Status = "KO",
                    Message = "Data not valid"
                });
            }
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        return SearchUser(model.Username, model.Password, context);
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

        //SOLO FASE TEST
        [HttpGet("GetAllDoctors", Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoctorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDoctors()
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var doctors = context.doctors.ToList();
                        if (doctors.Any())
                        {
                            return Ok(new DoctorResponse()
                            {
                                Status = "OK",
                                Data = doctors
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No doctors found"
                            });
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


        [Authorize(Roles = "DoctorAdmin,Doctor")]
        [HttpGet("GetAllDepartmentDoctors", Name = "GetAllDepartmentDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoctorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentDoctors(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindDoctorDepartment(doctorId, context);

                        var doctors = context.doctors.Where(x => x.Department == rightDepartment).ToList();
                        if (doctors.Any())
                        {
                            return Ok(new DoctorResponse()
                            {
                                Status = "OK",
                                Data = doctors
                            });

                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No doctors found in department {rightDepartment}"
                            });
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

        [Authorize(Roles = "DoctorAdmin,Doctor")]
        [HttpGet("GetAllDepartmentNurseFromDoctor", Name = "GetAllDepartmentNurseFromDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NurseResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentNurseFromDoctor(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindDoctorDepartment(doctorId, context);

                        var nurses = context.nurses.Where(x => x.Department == rightDepartment).ToList();
                        if (nurses.Any() && !String.IsNullOrEmpty(rightDepartment))
                        {
                            return Ok(new NurseResponse()
                            {
                                Status = "OK",
                                Data = nurses
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurses found in department {rightDepartment}"
                            });
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


        [AllowAnonymous]
        [HttpPost("AddDoctor", Name = "AddDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreateDoctor(Doctor doctor)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, doctor.Username);
                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Message = $"{doctor.Username}already exists in our database"
                            });

                        context.doctors.Add(doctor);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Message = $"Succesfully created a new Doctor: ${doctor.Username}"
                        };
                        return Ok(response);
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

        [Authorize(Roles = "DoctorAdmin")]
        [HttpPut("ModifyDoctor", Name = "ModifyDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyDoctor(Doctor doctor)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldDoctor = context.doctors.FirstOrDefault(x => x.ID == doctor.ID);
                        if (oldDoctor == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurse found with id {doctor.ID}"
                            });

                        var alreadyExist = UsernameAlreadyExist(context, doctor.Username);
                        if (alreadyExist)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"{doctor.Username} already exists in our database"
                            });
                        oldDoctor.Name = doctor.Name;
                        oldDoctor.Surname = doctor.Surname;
                        oldDoctor.Username = doctor.Username;
                        oldDoctor.Password = doctor.Password;
                        oldDoctor.Speciality = doctor.Speciality;
                        oldDoctor.Phone = doctor.Phone;
                        oldDoctor.Department = doctor.Department;
                        oldDoctor.Admin = doctor.Admin;

                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Doctor {doctor.ID} successfully modified "
                        });
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

        [Authorize(Roles = "DoctorAdmin")]
        [HttpDelete("DeleteDoctor", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult DeleteDoctor(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldDoctor = context.doctors.FirstOrDefault(x => x.ID == doctorId);
                        if (oldDoctor == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No doctor found with id {doctorId}"
                            });

                        context.doctors.Remove(oldDoctor);
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Doctor {doctorId} successfully deleted "
                        });
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

        //SOLO TEST
        [Authorize]
        [HttpGet("GetAllNurses", Name = "GetAllNurses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NurseResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllNurses()
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var nurses = context.nurses.ToList();
                        if (nurses.Any())
                        {
                            return Ok(new NurseResponse()
                            {
                                Status = "OK",
                                Data = nurses
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurses found"
                            });
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

        [Authorize(Roles = "NurseAdmin,Nurse")]
        [HttpGet("GetAllDepartmentNurses", Name = "GetAllDepartmentNurses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NurseResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentNurses(int nurseId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindNurseDepartment(nurseId, context);

                        var nurses = context.nurses.Where(x => x.Department == rightDepartment).ToList();
                        if (nurses.Any())
                        {
                            return Ok(new NurseResponse()
                            {
                                Status = "OK",
                                Data = nurses
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurses found in department {rightDepartment}"
                            });
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


        [Authorize(Roles = "NurseAdmin,Nurse")]
        [HttpGet("GetAllDepartmentDoctorsFromNurse", Name = "GetAllDepartmentDoctorsFromNurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoctorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentDoctorsFromNurse(int nurseId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindNurseDepartment(nurseId, context);

                        var doctors = context.doctors.Where(x => x.Department == rightDepartment).ToList();
                        if (doctors.Any())
                        {
                            return Ok(new DoctorResponse()
                            {
                                Status = "OK",
                                Data = doctors 
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No doctors found in department {rightDepartment}"
                            });
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


        [AllowAnonymous]
        [HttpPost("AddNurse", Name = "AddNurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreateNurse(Nurse nurse)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, nurse.Username);

                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Message = $"{nurse.Username} already exists in our database"
                            });

                        context.nurses.Add(nurse);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Message = $"{nurse.Username}Succesfully created a new Nurse"
                        };
                        return Ok(response);
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

        [Authorize(Roles = "NurseAdmin")]
        [HttpPut("ModifyNurse", Name = "ModifyNurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyNurse(Nurse nurse)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldNurse = context.nurses.FirstOrDefault(x => x.ID == nurse.ID);
                        if (oldNurse == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurse found with id {nurse.ID}"
                            });

                        var alreadyExist = UsernameAlreadyExist(context, nurse.Username);
                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Message = $"{nurse.Username} already exists in our database"
                            });

                        oldNurse.Name = nurse.Name;
                        oldNurse.Surname = nurse.Surname;
                        oldNurse.Username = nurse.Username;
                        oldNurse.Password = nurse.Password;
                        oldNurse.Phone = nurse.Phone;
                        oldNurse.Department = nurse.Department;
                        oldNurse.Admin = nurse.Admin;
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Nurse {nurse.ID} successfully modified "
                        });
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


        [Authorize(Roles = "NurseAdmin")]
        [HttpDelete("DeleteNurse", Name = "DeleteNurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult DeleteNurse(int nurseId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldNurse = context.nurses.FirstOrDefault(x => x.ID == nurseId);
                        if (oldNurse == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurse found with id {nurseId}"
                            });

                        context.nurses.Remove(oldNurse);
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Nurse {nurseId} successfully deleted "
                        });
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

        [Authorize(Roles = "DoctorAdmin,Doctor")]
        [HttpGet("GetAllPatients", Name = "GetAllPatients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllPatients()
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var patients = context.patients.ToList();
                        if (patients.Any())
                        {
                            return Ok(new PatientResponse()
                            {
                                Status = "OK",
                                Data= patients
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No patients found"
                            });
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


        [AllowAnonymous]
        [HttpPost("AddPatient", Name = "AddPatient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePostCreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponsePostCreateUser))]
        public IActionResult CreatePatient(Patient patient)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var alreadyExist = UsernameAlreadyExist(context, patient.Username);

                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Message = $"{patient.Username} already exists in our database"
                            });

                        context.patients.Add(patient);
                        context.SaveChanges();

                        var response = new ResponsePostCreateUser()
                        {
                            Status = "OK",
                            Message = $"{patient.Username} Succesfully created a new Patient"
                        };
                        return Ok(response);
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

        [Authorize(Roles = "DoctorAdmin,Doctor,NurseAdmin,Nurse")]
        [HttpPut("ModifyPatient", Name = "ModifyPatient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyPatient(Patient patient)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldPatient = context.patients.FirstOrDefault(x => x.ID == patient.ID);
                        if (oldPatient == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No nurse found with id {patient.ID}"
                            });
                        var alreadyExist = UsernameAlreadyExist(context, patient.Username);
                        if (alreadyExist)
                            return BadRequest(new ResponsePostCreateUser()
                            {
                                Status = "KO",
                                Message = $"{patient.Username} already exists in our database"
                            });

                        oldPatient.Name = patient.Name;
                        oldPatient.Surname = patient.Surname;
                        oldPatient.Username = patient.Username;
                        oldPatient.Password = patient.Password;
                        oldPatient.Age = patient.Age;
                        oldPatient.Address = patient.Address;
                        oldPatient.Phone = patient.Phone;
                       
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Patient {patient.ID} successfully modified "
                        });
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

        [Authorize(Roles = "DoctorAdmin,Doctor,NurseAdmin,Nurse")]
        [HttpDelete("DeletePatient", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult DeletePatient(int patientId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldPatient = context.patients.FirstOrDefault(x => x.ID == patientId);
                        if (oldPatient == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No patient found with id {patientId}"
                            });

                        context.patients.Remove(oldPatient);
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Patient {patientId} successfully deleted"
                        });
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
                            Message = "Invalid username or password."
                        });
                    else
                    {
                        var token = GenerateJwtToken(patient.ID, patient.Username, "Patient");
                        return Ok(new LoginResponse()
                        {

                            Status = "OK",
                            Message = $"{username} logged succesfully as patient",
                            Data = new LoginResponseData
                            {
                                Id = patient.ID,
                                Username = patient.Username,
                                Role = "Patient",
                                Token = token // Invio solo il token. nella richiesta inserisco davanti "Bearer "
                            }
                        });
                    }
                }
                else if (nurse.Admin)
                {
                    var token = GenerateJwtToken(nurse.ID, nurse.Username, "NurseAdmin");
                    return Ok(new LoginResponse()
                    {
                        Status = "OK",
                        Message = $"{username} logged succesfully as nurse",
                        Data = new LoginResponseData
                        {
                            Id = nurse.ID,
                            Username = nurse.Username,
                            Role = "AdminNurse",
                            Token = token // Invio solo il token. nella richiesta inserisco davanti "Bearer "
                        }
                    });
                }
                else
                {
                    var token = GenerateJwtToken(nurse.ID, nurse.Username, "Nurse");
                    return Ok(new LoginResponse()
                    {
                        Status = "OK",
                        Message = $"{username} logged succesfully as nurse",
                        Data = new LoginResponseData
                        {
                            Id = nurse.ID,
                            Username = nurse.Username,
                            Role = "Nurse",
                            Token = token // Invio solo il token. nella richiesta inserisco davanti "Bearer "
                        }
                    });
                }
            }
            else if (doctor.Admin)
            {
                var token = GenerateJwtToken(doctor.ID, doctor.Username, "DoctorAdmin");
                return Ok(new LoginResponse()
                {
                    Status = "OK",
                    Message = $"{username} logged succesfully as doctor",
                    Data = new LoginResponseData
                    {
                        Id = doctor.ID,
                        Username = doctor.Username,
                        Role = "DoctorAdmin",
                        Token = token // Invio solo il token. nella richiesta inserisco davanti "Bearer "
                    }
                });
            }
            else
            {
                var token = GenerateJwtToken(doctor.ID, doctor.Username, "Doctor");
                return Ok(new LoginResponse()
                {
                    Status = "OK",
                    Message = $"{username} logged succesfully as doctor",
                    Data = new LoginResponseData {
                        Id = doctor.ID,
                        Username = doctor.Username,
                        Role = "Doctor",
                        Token = token // Invio solo il token. nella richiesta inserisco davanti "Bearer "
                    }
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

        private static string? FindDoctorDepartment(int doctorId, HospitalDbContext context)
        {
            var doctor = context.doctors.FirstOrDefault(x => x.ID == doctorId);
            var rightDepartment = doctor?.Department;
            return rightDepartment;
        }
        private static string? FindNurseDepartment(int nurseId, HospitalDbContext context)
        {
            var nurse = context.nurses.FirstOrDefault(x => x.ID == nurseId);
            var rightDepartment = nurse?.Department;
            return rightDepartment;
        }

    }
}

