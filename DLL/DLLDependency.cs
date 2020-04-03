using DLL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DLL.UnitOfWork;

namespace DLL
{
  public class DLLDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            //services.AddTransient<IStudentRepository, StudentRepository>();
           // services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        }
    }
}
