using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly ILogger<RecordController> _logger;

        public RecordController(ILogger<RecordController> logger)
        {
            _logger = logger;
        }


        [Authorize]
        [HttpGet("GetAllPatientRecords", Name = "GetAllPatientRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllPatientRecords(int patientId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var records = context.records.Where(x => x.IDPatient == patientId);
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Records = records.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No records found for patient {patientId}"
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
        [HttpGet("GetAllDoctorRecords", Name = "GetAllDoctorRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDoctorRecords(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var records = context.records.Where(x => x.IDDoctor == doctorId);
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Records = records.ToList()
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
        [HttpGet("GetAllDepartmentDoctorRecords", Name = "GetAllDepartmentDoctorRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentDoctorRecords(int doctorId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindDoctorDepartment(doctorId, context);

                        var records = context.records.ToList();
                        if (records.Any() && !String.IsNullOrEmpty(rightDepartment))
                        {
                            List<Record> rightRecords = new List<Record>();
                            foreach (var item in records)
                            {
                                string? department = FindDoctorDepartment(item.IDDoctor, context);
                                if (department == rightDepartment)
                                    rightRecords.Add(item);

                            }
                            if (rightRecords.Count > 0)
                                return Ok(new RecordResponse()
                                {
                                    Status = "OK",
                                    Records = rightRecords
                                });
                            else
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = $"No records found for department {rightDepartment}"
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
        [HttpGet("GetAllNurseRecords", Name = "GetAllNurseRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllNurseRecords(int nurseId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var records = context.records.Where(x => x.IDNurse == nurseId);
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Records = records.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No records found for nurse {nurseId}"
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
        [HttpGet("GetAllDepartmentNurseRecords", Name = "GetAllDepartmentNurseRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllDepartmentNurseRecords(int nurseId)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        string? rightDepartment = FindNurseDepartment(nurseId, context);

                        var records = context.records.ToList();
                        if (records.Any() && !String.IsNullOrEmpty(rightDepartment))
                        {
                            List<Record> rightRecords = new List<Record>();
                            foreach (var item in records)
                            {
                                string? department = FindNurseDepartment(item.IDNurse, context);
                                if (department == rightDepartment)
                                    rightRecords.Add(item);

                            }
                            if(rightRecords.Count>0)
                                return Ok(new RecordResponse()
                                {
                                    Status = "OK",
                                    Records = rightRecords
                                });
                            else
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = $"No records found for department {rightDepartment}"
                                });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No records found for nurse {nurseId}"
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
        [HttpPost("CreateRecord", Name = "CreateRecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateRecord(Record record)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        context.records.Add(record);
                        context.SaveChanges();
                        return Ok(new GetResponse()
                        {
                            Status = "OK",
                            Message = "Record succesfully created"
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
        [HttpPut("ModifyRecords", Name = "ModifyRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyRecords(Record record)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var oldRecord = context.records.FirstOrDefault(x => x.ID == record.ID);
                        if (oldRecord == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No records found with id {record.ID}"
                            });
                        else
                        {
                            if((oldRecord.IDDoctor != record.IDDoctor) || (oldRecord.IDNurse != record.IDNurse))
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = $"You can't change Doctor/Nurse IDs"
                                });

                            oldRecord.Prescription = record.Prescription;
                            oldRecord.Diagnosis = record.Diagnosis;
                            oldRecord.Treatment = record.Treatment;

                            context.SaveChanges();

                            return Ok(new GetResponse()
                            {
                                Status = "OK",
                                Message = $"Record {record.ID} successfully modified "
                            });

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

        private static string? FindNurseDepartment(int nurseId, HospitalDbContext context)
        {
            var nurse = context.nurses.FirstOrDefault(x => x.ID == nurseId);
            var rightDepartment = nurse?.Department;
            return rightDepartment;
        }

    }
}
