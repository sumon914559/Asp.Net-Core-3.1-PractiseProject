using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Repository;

namespace DLL.UnitOfWork
{
  public interface IUnitOfWork
    {

        // All Repository need to add here 

            IDepartmentRepository DepartmentRepository { get; }
            IStudentRepository StudentRepository { get; }



        // All Repository need to add here End

        Task<bool> ApplicationSaveChanges();
        void Dispose();
    }

  public class UnitOfWork : IUnitOfWork,IDisposable
  {
      private readonly ApplicationDbContext _context;
      private bool disposed = false;

      private IDepartmentRepository _departmentRepository;
      private IStudentRepository _studentRepository;

      public UnitOfWork(ApplicationDbContext context)
      {
          _context = context;
      }

      public IDepartmentRepository DepartmentRepository =>
          _departmentRepository ??= new DepartmentRepository(_context);

      public IStudentRepository StudentRepository => 
          _studentRepository ??= new StudentRepository(_context);



      public async Task<bool> ApplicationSaveChanges()
      {
          return await _context.SaveChangesAsync() > 0 ? true : false;
        }

     protected virtual void Dispose(bool disposing)
      {
          if (!this.disposed)
          {
              if (disposing)
              {
                  _context.Dispose();
              }
              
          }

          this.disposed = true;
      }
        
    public void Dispose()
      {
         Dispose(disposing:true);
         GC.SuppressFinalize(this);
      }
  }

}
