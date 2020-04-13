using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
   
    public class AccountController : CommonApiController
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService AccountService)
        {
            _accountService = AccountService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            return Ok( await  _accountService.LoginUser(request));
        }
        [HttpGet("test1")]
        public ActionResult Test1()
        {
           return Ok("Enter Test1");
        }

        [Authorize]
        [HttpGet("test2")]
        public ActionResult Test2()
        {
            var tt = User;
            _accountService.Test(tt);
            return Ok("Enter Test2");
        }
    }
}