using BLL.Request;
using DLL.Model;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> AddStudentAsync(StudentRequest request)
        {
            Student student = new Student()
            {
                Name = request.Name,
                Roll = request.Roll,
                Email = request.Email
            }; 
           await _studentRepository.CreateAsync(student);
           if (await _studentRepository.ApplicationSaveChanges())
           {
               return student;
           }
           throw new MyApplicationExceptions("Student Data Not save");
        }

        public async Task<bool> DeleteAsync(string roll)
        {
            var student = await _studentRepository.GetAAsync(x => x.Roll == roll);
            if (student == null)
            {
                throw new MyApplicationExceptions("Roll wise Student not found");
            }

           _studentRepository.DeleteAsync(student);
           if (await _studentRepository.ApplicationSaveChanges())
           {
               return true;
           }

           return false;

        }

        public async Task<Student> GeatAStudentAsync(string roll)
        {
            var student = await _studentRepository.GetAAsync(x => x.Roll == roll);
            if(student == null)
            {
                throw new MyApplicationExceptions("Data Not found !!");
            }
            return student;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var allStudent = await _studentRepository.GetAllAsync();

             if (allStudent == null)
             {
                 throw new MyApplicationExceptions("No found Data");
             }

             return allStudent;
        }

        public async Task<bool> IsNameExit(string name)
        {
            var student = await _studentRepository.GetAAsync(x => x.Name == name);
            if (student != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsRollExit(string roll)
        {
            var student = await _studentRepository.GetAAsync(x => x.Roll == roll);
            if (student != null)
            {
                return false;
            }

            return true;
        }

        public async Task<Student> UpdateAsync(string roll, StudentUpdateRequest request)
        {

            var student = await _studentRepository.GetAAsync(x => x.Roll == roll);

            if (student == null)
            {
                throw new MyApplicationExceptions("No found Data");
            } 

            student.Name = request.Name;
            student.Roll = request.Roll;
            student.Email = request.Email;
            _studentRepository.UpdateAsync(student);
           

            if (await _studentRepository.ApplicationSaveChanges())
            {
                return student;
            }
            throw new MyApplicationExceptions("Student Data Not save");
        }
    }

}
