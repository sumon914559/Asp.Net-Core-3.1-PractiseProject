﻿using BLL.Request;
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
   public interface IStudentService
    {
        Task<Student> AddStudentAsync(StudentRequest request);
        Task<List<Student>> GetAllAsync();
        Task<Student> GeatAStudentAsync(string roll);
        Task<Student> UpdateAsync(string roll, StudentUpdateRequest request);
        Task<bool> DeleteAsync(string roll);

        Task<bool> IsNameExit(string name);
        Task<bool> IsRollExit(string roll);
    }

    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> AddStudentAsync(StudentRequest request)
        {
            Student student = new Student()
            {
                Name = request.Name,
                Roll = request.Roll,
                Email = request.Email,
                DepartmentId = request.DepartmentId
            };
            await _unitOfWork.StudentRepository.CreateAsync(student);
           if (await _unitOfWork.ApplicationSaveChanges())
           {
               return student;
           }
           throw new MyApplicationExceptions("Student Data Not save");
        }

        public async Task<bool> DeleteAsync(string roll)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.Roll == roll);
            if (student == null)
            {
                throw new MyApplicationExceptions("Roll wise Student not found");
            }

            _unitOfWork.StudentRepository.DeleteAsync(student);
           if (await _unitOfWork.ApplicationSaveChanges())
           {
               return true;
           }

           return false;

        }

        public async Task<Student> GeatAStudentAsync(string roll)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.Roll == roll);
            if(student == null)
            {
                throw new MyApplicationExceptions("Data Not found !!");
            }
            return student;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var allStudent = await _unitOfWork.StudentRepository.GetAllAsync();

             if (allStudent == null)
             {
                 throw new MyApplicationExceptions("No found Data");
             }

             return allStudent;
        }

        public async Task<bool> IsNameExit(string name)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.Name == name);
            if (student != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsRollExit(string roll)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.Roll == roll);
            if (student != null)
            {
                return false;
            }

            return true;
        }

        public async Task<Student> UpdateAsync(string roll, StudentUpdateRequest request)
        {

            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.Roll == roll);

            if (student == null)
            {
                throw new MyApplicationExceptions("No found Data");
            } 

            student.Name = request.Name;
            student.Roll = request.Roll;
            student.Email = request.Email;
            student.DepartmentId = request.DepartmentId;
            _unitOfWork.StudentRepository.UpdateAsync(student);
           

            if (await _unitOfWork.ApplicationSaveChanges())
            {
                return student;
            }
            throw new MyApplicationExceptions("Student Data Not save");
        }
    }

}
