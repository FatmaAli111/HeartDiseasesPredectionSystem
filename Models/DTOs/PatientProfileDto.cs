﻿using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
   public class PatientProfileDto
    {
        public string FullName { get; set; }
        public string? age { get; set; }
        public string? Description { get; set; }
        public Image Image { get; set; }
    }
}
