using BLL.Request;
using DLL.Model;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
   public interface IDepartmentServices
    {
        Task<Department> AddDepartmentAsync(DepartmentInsertRequest request);
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> GetADepartmentAsync(string Code);
        Task<Department> UpdateAsync(string Code, DepartmentInsertRequest request);
        Task<bool> DeleteAsync(string Code);

        Task<bool> IsCodeExit(string code);
        Task<bool> IsNameExit(string name);
    }

    public class DepartmentServices: IDepartmentServices
    {
        private readonly IDepartmentRepository _DepartmentRepository;
        public DepartmentServices(IDepartmentRepository DepartmentRepository)
        {
            _DepartmentRepository = DepartmentRepository;
        }

        public async Task<Department> AddDepartmentAsync(DepartmentInsertRequest request)
        {
            Department department = new Department()
            {
                Code = request.Code,
                Name = request.Name
            };
            return await _DepartmentRepository.AddDepartmentAsync(department);
        }

        public async Task<bool> DeleteAsync(string Code)
        {
            return await _DepartmentRepository.DeleteAsync(Code);
        }

        public async Task<Department> GetADepartmentAsync(string Code)
        {
            return await _DepartmentRepository.GetADepartmentAsync(Code);
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            return await _DepartmentRepository.GetAllDepartmentAsync();
        }

        public async Task<bool> IsCodeExit(string code)
        {
            return await _DepartmentRepository.IsCodeExit(code);
        }

        public async Task<bool> IsNameExit(string name)
        {
            return await _DepartmentRepository.IsNameExit(name);
        }

        public async Task<Department> UpdateAsync(string Code, DepartmentInsertRequest request)
        {
            Department department = new Department()
            {
                Code = request.Code,
                Name = request.Name
            };

            return await _DepartmentRepository.UpdateAsync(Code, department);
        }
    }
}
