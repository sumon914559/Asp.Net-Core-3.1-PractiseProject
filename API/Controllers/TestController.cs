using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;

namespace API.Controllers
{
   
    public class TestController : CommonApiController
    {
        private readonly TaposRSA _taposRsa;

        public TestController(TaposRSA taposRsa)
        {
            _taposRsa = taposRsa;
        }

        
        //[HttpGet]
        //public async Task<ActionResult> Test1()
        //{
        //   _taposRsa.GenerateRsaKey(version:"v1");
        //    return Ok("KeyGenerated");
        //}


        [HttpPost]
        public string Test(TestRequest request)
        {
            var rrn = request.customerRrn;

           /* var emails = _userManager.Users
                .Where(user => user.CustomerId == null)
                .Select(user => user.Email) // extract the emails from users
                .ToList();

            var customers = _applicationRepository.GetCustomers()
                .Where(customer => rrn.Contains(customer.Email)) // the Contains method carry the IN logic when translated to SQL script
                .ToList();*/





            return "hi";
        }

        [Authorize(Roles = "teacher", Policy = "AtToken")]
        [HttpGet]
        public async Task<ActionResult> Test2()
        {
           return Ok("Enter Test 2");
        }

    }
}