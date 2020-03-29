using DLL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
  public class DLLDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        }
    }
}
