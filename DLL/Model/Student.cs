﻿using System;
using System.Collections.Generic;
using System.Text;
using DLL.Model.Interfaces;

namespace DLL.Model
{
   public class Student : ITrackable, ISoftDelete
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Roll { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
