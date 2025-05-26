using Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public partial class DoctorProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string? Address { get; set; }
        public ImageDto Image { get; set; }
    }
}
