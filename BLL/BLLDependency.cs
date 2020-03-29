﻿using BLL.Request;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
  public class BLLDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddTransient<IDepartmentServices, DepartmentServices>();
            services.AddTransient<IStudentService, StudentService>();

            services.AddTransient<IValidator<DepartmentInsertRequest>, DepartmentInsertRequestValidator>();
            services.AddTransient<IValidator<StudentRequest>, StudentRequestValidator>();
        }
    }
}