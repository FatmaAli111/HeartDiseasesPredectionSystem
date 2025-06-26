using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AppointmentDto
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }

}
