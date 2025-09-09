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

        //SOLO TEST
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            }).ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data =appointments 
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


        [Authorize(Roles = "DoctorAdmin,Doctor")]
        [HttpPost("CreateAppointment", Name = "CreateAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        context.appointments.Add(appointment);
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

        [Authorize(Roles = "Patient")]
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDPatient == patientId
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            }).ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList() 
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

        [Authorize(Roles = "Patient")]
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDPatient == patientId && a.Date >= DateTime.Now
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            })
                        .OrderBy(x => x.Date)
                        .ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
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


        [Authorize(Roles = "Patient")]
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDPatient == patientId && a.Date <= DateTime.Now
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            })
                        .OrderByDescending(x => x.Date)
                        .ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
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

        [Authorize(Roles = "DoctorAdmin,Doctor,NurseAdmin,Nurse")]
        [HttpGet("GetAllDepartmentAppointments", Name = "GetAllDepartmentAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentAppointments(string department)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.Department == department
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            }).ToList();


                        if (appointments.Any())
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
                            });
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Appointments found for department {department}"
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            // LEFT JOIN per l'infermiere
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDDoctor == doctorId
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // o p.Name + " " + p.Surname 
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            }).ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
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


        [Authorize(Roles = "DoctorAdmin,Doctor")]
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDDoctor == doctorId && a.Date >= DateTime.Now
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            })
                        .OrderBy(x => x.Date)
                        .ToList();

                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
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


        [Authorize(Roles = "DoctorAdmin,Doctor")]
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
                        var appointments = (from a in context.appointments
                                            join p in context.patients on a.IDPatient equals p.ID
                                            join d in context.doctors on a.IDDoctor equals d.ID
                                            join n in context.nurses on a.IDNurse equals n.ID into nurseJoin
                                            from nurse in nurseJoin.DefaultIfEmpty()
                                            where a.IDDoctor == doctorId && a.Date <= DateTime.Now
                                            select new ViewAppoinment
                                            {
                                                ID = a.ID,
                                                IDPatient = a.IDPatient,
                                                PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                                IDNurse = a.IDNurse,
                                                NurseName = nurse != null ? nurse.Username : null,
                                                IDDoctor = a.IDDoctor,
                                                DoctorName = d.Username,
                                                Department = a.Department,
                                                Date = a.Date,
                                                Diagnosis = a.Diagnosis,
                                                Prescription = a.Prescription,
                                                Treatment = a.Treatment
                                            })
                        .OrderByDescending(x => x.Date)
                        .ToList();
                        
                        if (appointments.Any())
                        {
                            return Ok(new AppointmentResponse()
                            {
                                Status = "OK",
                                Data = appointments.ToList()
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

        [Authorize(Roles = "DoctorAdmin,Doctor")]
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
                        oldAppointment.Department = appointment.Department;
                        oldAppointment.Date = appointment.Date;
                        oldAppointment.Diagnosis = appointment.Diagnosis;
                        oldAppointment.Prescription = appointment.Prescription;
                        oldAppointment.Treatment = appointment.Treatment;
                        
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

        [Authorize(Roles = "DoctorAdmin,Doctor")]
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
    }
}
