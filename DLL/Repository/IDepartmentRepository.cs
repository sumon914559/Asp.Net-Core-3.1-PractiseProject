using DLL.DbContext;
using DLL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.UnitOfWork;


namespace DLL.Repository
{
   public interface IDepartmentRepository: IRepositoryBase<Department>
    {
       

    }

    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
                
        } 
    }
}
