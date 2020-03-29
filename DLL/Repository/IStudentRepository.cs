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
   public interface IStudentRepository
    {
        Task<Student> AddStudentAsync(Student aStudent);
        Task<List<Student>> GetAllAsync();
        Task<Student> GeatAStudentAsync(string roll);
        Task<Student> UpdateAsync(string roll, Student aStudent);
        Task<bool> DeleteAsync(string roll);

        Task<bool> IsNameExit(string name);
        Task<bool> IsRollExit(string roll);

    }

    public class StudentRepository: IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> AddStudentAsync(Student aStudent)
        {
           await _context.Students.AddAsync(aStudent);
            await _context.SaveChangesAsync();
            return aStudent;
        }

        public async Task<bool> DeleteAsync(string roll)
        {
            var student = await _context.Students
         .FirstOrDefaultAsync(s => s.Roll == roll);
            if(student.StudentId > 0)
            {
                _context.Remove(student);
               await  _context.SaveChangesAsync();
                return true;
            }
            return false;
            
        }

       

        public async Task<Student> GeatAStudentAsync(string roll)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Roll == roll);
            return student;
        }

        public async Task< List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<bool> IsNameExit(string name)
        {
            var IsName = await _context.Students.FirstOrDefaultAsync(x => x.Name == name);
            if(IsName == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsRollExit(string roll)
        {
            var IsRoll = await _context.Students.FirstOrDefaultAsync(x => x.Roll == roll);
            if (IsRoll == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Student> UpdateAsync(string roll, Student aStudent)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(item => item.Roll == roll);

            if (entity != null)
            {
                entity.Name = aStudent.Name;
                entity.Email = aStudent.Email;
                _context.Students.Update(entity);
               await _context.SaveChangesAsync();
            }
            return aStudent;
        }
    }
}
