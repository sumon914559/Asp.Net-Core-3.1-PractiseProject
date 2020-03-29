using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class DepartmentController : CommonApiController
    {
        private readonly IDepartmentServices _DepartmentServices;
        public DepartmentController(IDepartmentServices DepartmentServices)
        {
            _DepartmentServices = DepartmentServices;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllDepartment()
        {
            return Ok(await _DepartmentServices.GetAllDepartmentAsync());
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> GetADepartment(string code)
        {
            return Ok(await _DepartmentServices.GetADepartmentAsync(code));
        }
        [HttpPost]
        public async Task<ActionResult> AddDepartment(DepartmentInsertRequest request)
        {
            return Ok(await _DepartmentServices.AddDepartmentAsync(request)); 
        }

        [HttpPut("{code}")]
        public async Task<ActionResult> Update(string code, DepartmentInsertRequest request)
        {
            return Ok(await _DepartmentServices.UpdateAsync(code, request));
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> Delete(string code)
        {
            return Ok(await _DepartmentServices.DeleteAsync(code));
        }

    }
}