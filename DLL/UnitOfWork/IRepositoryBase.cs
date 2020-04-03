using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DLL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DLL.UnitOfWork
{
  public  interface IRepositoryBase<T> where  T:class
  {
      Task CreateAsync(T entity);
      void UpdateAsync(T entity);
      void DeleteAsync(T entity);
      Task <T>GetAAsync(Expression<Func<T, bool>> expression = null);
      Task<List<T>> GetAllAsync();
      
  }

  public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task CreateAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> GetAAsync(Expression<Func<T, bool>> expression = null)
        {
          return await _context.Set<T>().FirstOrDefaultAsync(expression);
        } 

        public async Task <List<T>> GetAllAsync()
        {
           return  await _context.Set<T>().ToListAsync();
        }

        public void UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

    }


}
