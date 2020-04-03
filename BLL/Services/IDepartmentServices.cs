using BLL.Request;
using DLL.Model;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DLL.UnitOfWork;
using Microsoft.VisualBasic;
using Utility.Exceptions;

namespace BLL.Services
{
   public interface IDepartmentServices
    {
        Task<Department> AddDepartmentAsync(DepartmentInsertRequest request);
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> GetADepartmentAsync(string code);
        Task<Department> UpdateAsync(string code, DepartmentInsertRequest request);
        Task<bool> DeleteAsync(string code);

        Task<bool> IsCodeExit(string code);
        Task<bool> IsNameExit(string name);
    }

    public class DepartmentServices: IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Department> AddDepartmentAsync(DepartmentInsertRequest request)
        {
            Department department = new Department()
            {
                Code = request.Code,
                Name = request.Name
            };

          await _unitOfWork.DepartmentRepository.CreateAsync(department);
          if (await _unitOfWork.ApplicationSaveChanges())
          {
              return department;
          }

          throw new MyApplicationExceptions("something went wrong");
        }

        public async Task<bool> DeleteAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAAsync(s => s.Code == code);
            if (department == null)
            {
                throw new MyApplicationExceptions("Department Not found!");
            }
            _unitOfWork.DepartmentRepository.DeleteAsync(department);

            if (await _unitOfWork.ApplicationSaveChanges())
            {
                return true;
            }

            return false;
        }

        public async Task<Department> GetADepartmentAsync(string code)
        {
            var department =   await _unitOfWork.DepartmentRepository.GetAAsync(x=>x.Code ==code);
           
            if (department == null)
            {
                throw new MyApplicationExceptions("Department Not found!");
            }

            return department;
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
           var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            if (department == null)
            {
                throw new MyApplicationExceptions("Department Not found!");
            }

            return department;
        }

        public async Task<bool> IsCodeExit(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Code == code);
            if (department != null)
            {
                return false;
                
            }
            return true;

        }

        public async Task<bool> IsNameExit(string name)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Name == name);
            if (department != null)
            {
                return false;
            }
            return true;
           
        }

        public async Task<Department> UpdateAsync(string code, DepartmentInsertRequest request)
        {
            //Department department = new Department()
            //{
            //    Code = request.Code,
            //    Name = request.Name
            //};

            var getADepartmentData = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Code == code);
           
            if (getADepartmentData == null)
            {
                throw new MyApplicationExceptions("Department Data found!");
            }

            //if (!string.IsNullOrWhiteSpace(request.Code))
            //{
            //    var isCodeChanged = await _DepartmentRepository.GetAAsync(x => x.Code == request.Code && x.DepartmentId == getADepartmentData.DepartmentId);
            //    if (isCodeChanged == null)
            //    {
            //        getADepartmentData.Code = request.Code;
            //    }
            //    else
            //    {
            //        throw new MyApplicationExceptions("Code Already exits,Put different Code.");
            //    }

            //}

            //if (!string.IsNullOrWhiteSpace(request.Name))
            //{
            //    var isNameChanged = await _DepartmentRepository.GetAAsync(x => x.Name == request.Name && x.DepartmentId == getADepartmentData.DepartmentId);

            //    if (isNameChanged == null)
            //    {
            //        getADepartmentData.Name = request.Name;
            //    }
            //    else
            //    {
            //        throw new MyApplicationExceptions("Name Already exists,Put different Code.");
            //    }

            //}

            if (!string.IsNullOrWhiteSpace(request.Code) && !string.IsNullOrWhiteSpace(request.Name))
            {
                getADepartmentData.Code = request.Code;
                getADepartmentData.Name = request.Name;
                _unitOfWork.DepartmentRepository.UpdateAsync(getADepartmentData);
            }


            if (await _unitOfWork.ApplicationSaveChanges())
            {
                return getADepartmentData;
            }

            throw new MyApplicationExceptions("something went wrong");
        }
    }
}
