using DLL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.DbContext
{
    public class ApplicationDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
