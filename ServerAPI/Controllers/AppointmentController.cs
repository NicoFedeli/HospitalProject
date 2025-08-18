using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetAllAppointments", Name = "GetAllAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllAppointments()
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.ToList();
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Appointments in Db"
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


        [Authorize]
        [HttpPost("CreateAppointment", Name = "CreateAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            try
            {
                var newAppointment = new Appointment()
                {
                    IDPatient = appointment.IDPatient,
                    IDDoctor = appointment.IDDoctor,
                    Date = appointment.Date
                };
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        context.appointments.Add(newAppointment);
                        context.SaveChanges();
                        var response = new GetResponse()
                        {
                            Status = "OK",
                            Message = "Appointment succesfully created"
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

        [Authorize]
        [HttpGet("GetAllPatientAppointments", Name = "GetAllPatientAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllPatientAppointments(int patientId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.Where(x => x.IDPatient == patientId);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Appointments for patient {patientId} in Db"
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

        [Authorize]
        [HttpGet("GetFuturePatientAppointments", Name = "GetFuturePatientAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetFuturePatientAppointments(int patientId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.OrderBy(x => x.Date).Where(x => x.IDPatient == patientId && x.Date >= DateTime.Now);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Future Appointments for patient {patientId} in Db"
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


        [Authorize]
        [HttpGet("GetPastPatientAppointments", Name = "GetPastPatientAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetPastPatientAppointments(int patientId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.OrderByDescending(x => x.Date).Where(x => x.IDPatient == patientId && x.Date <= DateTime.Now);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Future Appointments for patient {patientId} in Db"
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

        [Authorize]
        [HttpGet("GetAllDepartmentAppointments", Name = "GetAllDepartmentAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentAppointments(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindDoctorDepartment(doctorId, context);

                        var appointments = context.appointments.Where(x => x.IDDoctor == doctorId).ToList();
                        if (appointments.Any())
                        {
                            List<Appointment> rightAppointments = new List<Appointment>();
                            foreach (var item in appointments)
                            {
                                string? department = FindDoctorDepartment(item.IDDoctor, context);
                                if (department == rightDepartment)
                                    rightAppointments.Add(item);

                            }
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = rightAppointments
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No records found for doctor {doctorId}"
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

        [Authorize]
        [HttpGet("GetAllDoctorAppointments", Name = "GetAllDoctorAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDoctorAppointments(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.Where(x => x.IDDoctor == doctorId);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Appointments for doctor {doctorId} in Db"
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


        [Authorize]
        [HttpGet("GetFutureDoctorAppointments", Name = "GetFutureDoctorAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetFutureDoctorAppointments(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.OrderBy(x => x.Date).Where(x => x.IDDoctor == doctorId && x.Date >= DateTime.Now);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Future Appointments for doctor {doctorId} in Db"
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


        [Authorize]
        [HttpGet("GetPastDoctorAppointments", Name = "GetPastDoctorAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetPastDoctorAppointments(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = context.appointments.OrderByDescending(x => x.Date).Where(x => x.IDPatient == doctorId && x.Date <= DateTime.Now);
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Appointments = appointments.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Future Appointments for doctor {doctorId} in Db"
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

        [Authorize]
        [HttpPut("ModifyAppointment", Name = "ModifyApointment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyAppointment(Appointment appointment)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldAppointment = context.appointments.FirstOrDefault(x => x.ID == appointment.ID);
                        if (oldAppointment == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No appointments found with id {appointment.ID}"
                            });

                        oldAppointment.IDPatient = appointment.IDPatient;
                        oldAppointment.IDDoctor = appointment.IDDoctor;
                        oldAppointment.Date = appointment.Date;

                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Appointment {appointment.ID} successfully modified "
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

        [Authorize]
        [HttpDelete("DeleteAppointment", Name = "DeleteAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult DeleteAppointment(int appointmentId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldAppointment = context.appointments.FirstOrDefault(x => x.ID == appointmentId);
                        if (oldAppointment == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No appointments found with id {appointmentId}"
                            });

                        context.appointments.Remove(oldAppointment);
                        context.SaveChanges();

                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = $"Appointment {appointmentId} successfully deleted "
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

        private static string? FindDoctorDepartment(int doctorId, HospitalDbContext context)
        {
            var doctor = context.doctors.FirstOrDefault(x => x.ID == doctorId);
            var rightDepartment = doctor?.Department;
            return rightDepartment;
        }
    }
}
