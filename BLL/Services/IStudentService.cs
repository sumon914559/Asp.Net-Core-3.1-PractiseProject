using BLL.Request;
using DLL.Model;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
   public interface IStudentService
    {
        Task<Student> AddStudentAsync(StudentRequest request);
        Task<List<Student>> GetAllAsync();
        Task<Student> GeatAStudentAsync(string roll);
        Task<Student> UpdateAsync(string roll, StudentRequest request);
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
            return await _studentRepository.AddStudentAsync(student);
        }

        public async Task<bool> DeleteAsync(string roll)
        {
            return await _studentRepository.DeleteAsync(roll);
        }

        public async Task<Student> GeatAStudentAsync(string roll)
        {
            return await _studentRepository.GeatAStudentAsync(roll);
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<bool> IsNameExit(string name)
        {
            return await _studentRepository.IsNameExit(name);
        }

        public async Task<bool> IsRollExit(string roll)
        {
            return await _studentRepository.IsRollExit(roll);
        }

        public async Task<Student> UpdateAsync(string roll, StudentRequest request)
        {
            Student student = new Student()
            {
                Name = request.Name,
                Roll = request.Roll,
                Email = request.Email
            };

            return await _studentRepository.UpdateAsync(roll, student);
        }
    }

}
