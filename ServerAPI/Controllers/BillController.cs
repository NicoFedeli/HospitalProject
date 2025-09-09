using HospitalAPI.Models;
using HospitalAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly ILogger<BillController> _logger;

        public BillController(ILogger<BillController> logger)
        {
            _logger = logger;
        }

        //ritorna tutti i ticket di un paziente specifico
        [Authorize(Roles = "Patient")]
        [HttpGet("GetAllPatientBills", Name = "GetAllBillsByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult Get(int id)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var bills = context.bills.Where(x => x.IDPatient == id);
                        if (bills.Any())
                        {
                            return Ok(new BillResponse()
                            {
                                Status = "OK",
                                Data = bills.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"No Bills found for Patient {id}"
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

        //Ritorna tutti i ticket non pagati di un utente specifico
        [Authorize(Roles = "Patient")]
        [HttpGet("GetNotPaidPatientBills", Name = "GetNotPaidBillsByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetNotpaid(int id)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var bills = context.bills.Where(x => x.IDPatient == id && x.Status == Constants.BillStatusUnpaid);
                        if (bills.Any())
                        {
                            return Ok(new BillResponse()
                            {
                                Status = "OK",
                                Data = bills.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"All bill has been paid for Patient {id}"
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

        //Ritorna tutti i ticket già pagati di uno specifico paziente
        [Authorize(Roles = "Patient")]
        [HttpGet("GetPaidPatientBills", Name = "GetPaidBillsByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult GetPaid(int id)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var bills = context.bills.Where(x => x.IDPatient == id && x.Status == Constants.BillStatusPaid);
                        if (bills.Any())
                        {
                            return Ok(new BillResponse()
                            {
                                Status = "OK",
                                Data = bills.ToList()
                            });
                        }
                        else
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = $"All bill has been paid for Patient {id}"
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

        //Creazione di un ticket
        [Authorize(Roles = "DoctorAdmin")]
        [HttpPost("CreateBill", Name = "CreateBill")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateBill(Bill bill)
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.bills.Add(bill);
                            context.SaveChanges();
                            transaction.Commit();
                            var response = new GetResponse()
                            {
                                Status = "OK",
                                Message = "Bill succesfully created"
                            };
                            return Ok(response);
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


        //Permette il pagamento di un ticket cambiandolo di stato
        [Authorize(Roles = "Patient")]
        [HttpPatch("PayBill", Name = "PayBillByIds")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult PayBill(int id, int patientId) 
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //controllo che esista il ticket
                            var bill = context.bills.FirstOrDefault(x => x.ID == id && x.IDPatient == patientId);

                            if (bill == null)
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = "Bill not found"
                                });
                            else
                            {
                                //se esiste controllo che non sia gia pagato
                                if (bill.Status == Constants.BillStatusPaid)
                                    return BadRequest(new GetResponse()
                                    {
                                        Status = "KO",
                                        Message = $"Bill {id} has been already paid"
                                    }); ;

                                //cambio lo stato da non pagato a pagato
                                bill.Status = Constants.BillStatusPaid;
                                context.SaveChanges();
                                transaction.Commit();
                                return Ok(new GetResponse()
                                {
                                    Status = "OK",
                                    Message = $"Bill {id} successfully paid"
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
                            }); ;
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
                }); ;
            }
        }
    }
}
