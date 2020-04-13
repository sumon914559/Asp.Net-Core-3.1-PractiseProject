using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class StudentController : CommonApiController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
           _studentService = studentService;
        }

        [Authorize(Roles = "teacher")]
        [HttpGet]
        public async Task<ActionResult> GetAllStudnet()
        {
            return Ok(await _studentService.GetAllAsync());
        }

        [Authorize(Roles = "teacher")]
        [HttpGet("{roll}")]
        public async Task<ActionResult> GetAStudent(string roll)
        {
            return Ok( await _studentService.GeatAStudentAsync(roll));
        }

        [Authorize(Roles = "teacher")]
        [HttpPost]
        public async Task<ActionResult> AddStudent([FromForm] StudentRequest request)
        {
            return Ok(await _studentService.AddStudentAsync(request));
        }

        [Authorize(Roles = "staff")]
        [HttpPut("{roll}")]
        public async Task<ActionResult> Update(string roll,[FromForm]  StudentUpdateRequest request)
        {
            return Ok(await _studentService.UpdateAsync(roll, request));
        }

        [Authorize(Roles = "teacher")]
        [HttpDelete("{roll}")]
        public async Task<ActionResult> Delete(string roll)
        {
            return Ok( await _studentService.DeleteAsync(roll));
        }

        
    }
}