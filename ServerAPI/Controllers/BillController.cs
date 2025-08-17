using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HospitalAPI.Models;
using HospitalAPI.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public BillController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [Authorize]
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
                                Bills = bills.ToList()
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

        [Authorize]
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
                                Bills = bills.ToList()
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



        [Authorize]
        [HttpPost("CreateBill", Name = "CreateBill")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult CreateBill(Bill bill)
        {
            try
            {
                var newBill = new Bill()
                {
                    IDPatient = bill.IDPatient,
                    Amount = bill.Amount,
                    Status = bill.Status
                };
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        context.bills.Add(newBill);
                        context.SaveChanges();
                        var response = new GetResponse()
                        {
                            Status = "OK",
                            Message = "Bill succesfully created"
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
        [HttpPatch("PayBill", Name = "PayBillByIds")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse))]
        public IActionResult PayBill(int id, int patientId) 
        {
            try
            {
                using (var context = new HospitalDbContext())
                {
                    try
                    {
                        var bill = context.bills.FirstOrDefault(x => x.ID == id && x.IDPatient == patientId);

                        if (bill == null)
                            return BadRequest(new GetResponse()
                            {
                                Status = "KO",
                                Message = "Bill not found"
                            });
                        else
                        {
                            if(bill.Status == Constants.BillStatusPaid)
                                return BadRequest(new GetResponse()
                                {
                                    Status = "KO",
                                    Message = $"Bill {id} has been already paid"
                                }); ;

                            bill.Status = Constants.BillStatusPaid;
                            context.SaveChanges();

                            return Ok(new GetResponse()
                            {
                                Status = "OK",
                                Message = $"Bill {id} paid successfully"
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
                        }); ;
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
