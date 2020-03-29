using DLL.DbContext;
using DLL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repository
{
   public interface IDepartmentRepository
    {
        Task<Department> AddDepartmentAsync(Department aDepartment);
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> GetADepartmentAsync(string DepartmentCode);
        Task<Department> UpdateAsync(string DepartmentCode, Department aDepartment);
        Task<bool> DeleteAsync(string DepartmentCode);

        Task<bool> IsCodeExit(string code);
        Task<bool> IsNameExit(string name);

    }

  public class DepartmentRepository: IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department> AddDepartmentAsync(Department aDepartment)
        {
           await _context.Departments.AddAsync(aDepartment);
           await _context.SaveChangesAsync();
            return aDepartment;
        }

        public async Task<bool> DeleteAsync(string DepartmentCode)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.Code == DepartmentCode);
            if (department.DepartmentId > 0)
            {
                _context.Departments.Remove(department);
               await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Department> GetADepartmentAsync(string DepartmentCode)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Code == DepartmentCode);
            return department;
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            return await _context.Departments.ToListAsync(); 
        }

        public async Task<bool> IsCodeExit(string code)
        {
            var deptCode = await _context.Departments.FirstOrDefaultAsync(d => d.Code == code);
            if(deptCode == null)
            {
                return false; 
            }
            return true ;
        }

        public async Task<bool> IsNameExit(string name)
        {
            var deptName = await _context.Departments.FirstOrDefaultAsync(d => d.Name == name);
            if (deptName == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Department> UpdateAsync(string DepartmentCode, Department aDepartment)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Code == DepartmentCode);
            if(department.DepartmentId > 0)
            {
                department.Code = aDepartment.Code;
                department.Name = aDepartment.Name;
                _context.Departments.Update(department);
              await  _context.SaveChangesAsync();
                return aDepartment;
            }
            return null; 
        }
    }
}
