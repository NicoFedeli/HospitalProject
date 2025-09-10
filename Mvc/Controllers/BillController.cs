using System.Security.Claims;
using Hospital.Models.Bill;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class BillController:Controller
    {
        private readonly IApiHelper _api;

        public BillController(IApiHelper api)
        {
            _api = api;
        }


    }
}
