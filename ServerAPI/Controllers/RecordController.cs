using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

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

        // Ritorna Tutti i record
        [Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor,Nurse,Patient")]
        [HttpGet("GetAllRecords", Name = "GetAllRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetAllPatientRecordsDetailed()
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var records = (from r in context.records
                                       join p in context.patients on r.IDPatient equals p.ID
                                       join d in context.doctors on r.IDDoctor equals d.ID
                                       join n in context.nurses on r.IDNurse equals n.ID into nurseJoin
                                       from nurse in nurseJoin.DefaultIfEmpty()
                                       select new ViewRecord
                                       {
                                           ID = r.ID,
                                           IDPatient = r.IDPatient,
                                           PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                           IDDoctor = r.IDDoctor,
                                           DoctorName = d.Username,
                                           IDNurse = r.IDNurse,
                                           NurseName = nurse != null ? nurse.Username : null,
                                           Diagnosis = r.Diagnosis,
                                           Prescription = r.Prescription,
                                           Treatment = r.Treatment
                                       }).ToList();

                        if (records.Any())
                        {
                            return Ok(new RecordResponse
                            {
                                Status = "OK",
                                Data = records
                            });
                        }
                        else
                        {
                            return BadRequest(new GetResponse
                            {
                                Status = "KO",
                                Message = $"No records found"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        return BadRequest(new GetResponse
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
                return BadRequest(new GetResponse
                {
                    Status = "KO",
                    Message = ex.Message
                });
            }
        }


        //Ritorna tutte le ricette di un paziente specifico passato con id
        [Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor,Nurse,Patient")]
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
                        var records = (from r in context.records
                                       join p in context.patients on r.IDPatient equals p.ID
                                       join d in context.doctors on r.IDDoctor equals d.ID
                                       join n in context.nurses on r.IDNurse equals n.ID into nurseJoin
                                       from nurse in nurseJoin.DefaultIfEmpty()
                                       where r.IDPatient == patientId
                                       select new ViewRecord
                                       {
                                           ID = r.ID,
                                           IDPatient = r.IDPatient,
                                           PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                           IDDoctor = r.IDDoctor,
                                           DoctorName = d.Username,
                                           IDNurse = r.IDNurse,
                                           NurseName = nurse != null ? nurse.Username : null,
                                           Diagnosis = r.Diagnosis,
                                           Prescription = r.Prescription,
                                           Treatment = r.Treatment
                                       }).ToList();
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Data = records.ToList()
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

        //ritorna tutte le ricette in cui e presente il medico passato tramite id
        [Authorize(Roles = "DoctorAdmin,Doctor")]
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
                        var records = (from r in context.records
                                       join p in context.patients on r.IDPatient equals p.ID
                                       join d in context.doctors on r.IDDoctor equals d.ID
                                       join n in context.nurses on r.IDNurse equals n.ID into nurseJoin
                                       from nurse in nurseJoin.DefaultIfEmpty()
                                       where r.IDDoctor == doctorId
                                       select new ViewRecord
                                       {
                                           ID = r.ID,
                                           IDPatient = r.IDPatient,
                                           PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                           IDDoctor = r.IDDoctor,
                                           DoctorName = d.Username,
                                           IDNurse = r.IDNurse,
                                           NurseName = nurse != null ? nurse.Username : null,
                                           Diagnosis = r.Diagnosis,
                                           Prescription = r.Prescription,
                                           Treatment = r.Treatment
                                       }).ToList();
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Data = records.ToList()
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

        // Commento perché non utilizzato
        //ritorna tutte le ricette di un determinato reparto in base al dottore passato con id
        //[Authorize(Roles = "DoctorAdmin")]
        //[HttpGet("GetAllDepartmentDoctorRecords", Name = "GetAllDepartmentDoctorRecords")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        //public IActionResult GetAllDepartmentDoctorRecords(int doctorId)
        //{
        //    try
        //    {
        //        using (var context = new HospitalDbContext())
        //        {
        //            try
        //            {
        //                //vado a prendere il dipartimento del dottore
        //                string? rightDepartment = FindDoctorDepartment(doctorId, context);

        //                //vado a prendermi tutte le ricette esistenti
        //                var records = context.records.ToList();
        //                if (records.Any() && !String.IsNullOrEmpty(rightDepartment))
        //                {
        //                    List<Record> rightRecords = new List<Record>();
        //                    foreach (var item in records)
        //                    {
        //                        //vado a cercare il dipartimento del dottore presente in ogni ricetta
        //                        string? department = FindDoctorDepartment(item.IDDoctor, context);
        //                        //se corrisponde con quello del dottore che gli ho passato se lo salva per poi tornarlo
        //                        if (department == rightDepartment)
        //                            rightRecords.Add(item);

        //                    }
        //                    if (rightRecords.Count > 0)
        //                        return Ok(new RecordResponse()
        //                        {
        //                            Status = "OK",
        //                            Data = rightRecords
        //                        });
        //                    else
        //                        return BadRequest(new GetResponse()
        //                        {
        //                            Status = "KO",
        //                            Message = $"No records found for department {rightDepartment}"
        //                        });
        //                }
        //                else
        //                    return BadRequest(new GetResponse()
        //                    {
        //                        Status = "KO",
        //                        Message = $"No records found for doctor {doctorId}"
        //                    });
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                Console.WriteLine(ex.StackTrace);
        //                return BadRequest(new GetResponse()
        //                {
        //                    Status = "KO",
        //                    Message = ex.Message
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.StackTrace);
        //        return BadRequest(new GetResponse()
        //        {
        //            Status = "KO",
        //            Message = ex.Message
        //        });
        //    }
        //}

        //ritorna tutte le ricette dove una infermiera ha partecipato passata tramite id
        [Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor,Nurse")]
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
                        var records = (from r in context.records
                                       join p in context.patients on r.IDPatient equals p.ID
                                       join d in context.doctors on r.IDDoctor equals d.ID
                                       join n in context.nurses on r.IDNurse equals n.ID into nurseJoin
                                       from nurse in nurseJoin.DefaultIfEmpty()
                                       where r.IDNurse == nurseId
                                       select new ViewRecord
                                       {
                                           ID = r.ID,
                                           IDPatient = r.IDPatient,
                                           PatientName = p.Username, // oppure p.Name + " " + p.Surname
                                           IDDoctor = r.IDDoctor,
                                           DoctorName = d.Username,
                                           IDNurse = r.IDNurse,
                                           NurseName = nurse != null ? nurse.Username : null,
                                           Diagnosis = r.Diagnosis,
                                           Prescription = r.Prescription,
                                           Treatment = r.Treatment
                                       }).ToList();
                        if (records.Any())
                        {
                            return Ok(new RecordResponse()
                            {
                                Status = "OK",
                                Data = records.ToList()
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

        // Commento perché non utilizzato
        //ritorna tutte le ricette di un reparto di un infermiera specifica passata tramite id
        //[Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor")]
        //[HttpGet("GetAllDepartmentNurseRecords", Name = "GetAllDepartmentNurseRecords")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        //public IActionResult GetAllDepartmentNurseRecords(int nurseId)
        //{
        //    try
        //    {
        //        using (var context = new HospitalDbContext())
        //        {
        //            try
        //            {
        //                //trovo il dipartimento della infermiera
        //                string? rightDepartment = FindNurseDepartment(nurseId, context);

        //                //prendo tute le ricette
        //                var records = context.records.ToList();
        //                if (records.Any() && !String.IsNullOrEmpty(rightDepartment))
        //                {
        //                    List<Record> rightRecords = new List<Record>();
        //                    foreach (var item in records)
        //                    {
        //                        //per ogni ricetta guardo il dipartimento dell infermiera
        //                        string? department = FindNurseDepartment(item.IDNurse, context);
        //                        // se il dipartimento è uguale salvo la ricetta per ritornarla
        //                        if (department == rightDepartment)
        //                            rightRecords.Add(item);

        //                    }
        //                    if (rightRecords.Count > 0)
        //                        return Ok(new RecordResponse()
        //                        {
        //                            Status = "OK",
        //                            Data = rightRecords
        //                        });
        //                    else
        //                        return BadRequest(new GetResponse()
        //                        {
        //                            Status = "KO",
        //                            Message = $"No records found for department {rightDepartment}"
        //                        });
        //                }
        //                else
        //                    return BadRequest(new GetResponse()
        //                    {
        //                        Status = "KO",
        //                        Message = $"No records found for nurse {nurseId}"
        //                    });
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                Console.WriteLine(ex.StackTrace);
        //                return BadRequest(new GetResponse()
        //                {
        //                    Status = "KO",
        //                    Message = ex.Message
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.StackTrace);
        //        return BadRequest(new GetResponse()
        //        {
        //            Status = "KO",
        //            Message = ex.Message
        //        });
        //    }
        //}

        //Creazione di una nuova ricetta
        [Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor")]
        [HttpPost("CreateRecord", Name = "CreateRecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateRecord(Record record)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //non ho messo controlli sugli id perche vengono gia fatti lato db avendo inserito foreign key
                            context.records.Add(record);
                            context.SaveChanges();
                            transaction.Commit();
                            return Ok(new GetResponse()
                            {
                                Status = "OK",
                                Message = "Record succesfully created"
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
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

        //Modifica di una ricetta
        [Authorize(Roles = "DoctorAdmin,NurseAdmin,Doctor")]
        [HttpPut("ModifyRecords", Name = "ModifyRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult ModifyRecords(Record record)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //controllo che esista la ricetta da modificare
                            var oldRecord = context.records.FirstOrDefault(x => x.ID == record.ID);
                            if (oldRecord == null)
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = $"No records found with id {record.ID}"
                                });
                            else
                            {
                                //controllo che gli id del dottore, infermiere e paziente non sia cambiato per evitare manomissioni alle cartelle cliniche
                                if ((oldRecord.IDDoctor != record.IDDoctor) || (oldRecord.IDNurse != record.IDNurse) || (oldRecord.IDPatient != record.IDPatient))
                                    return BadRequest(new GetResponse()
                                    {
                                        Status = "KO",
                                        Message = $"You can't change Doctor/Nurse IDs"
                                    });

                                //sostituisco solo i valori possibili da modificare
                                NewRecord(record, oldRecord);

                                context.SaveChanges();
                                transaction.Commit();

                                return Ok(new GetResponse()
                                {
                                    Status = "OK",
                                    Message = $"Record {record.ID} successfully modified "
                                });

                            }


                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
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

        //NON ESISTE UNA DELETE PERCHE LE CARTELLE CLINICHE RIMANGONO PER AVERE LO STORICO 
        private static void NewRecord(Record record, Record oldRecord)
        {
            oldRecord.Prescription = record.Prescription;
            oldRecord.Diagnosis = record.Diagnosis;
            oldRecord.Treatment = record.Treatment;
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