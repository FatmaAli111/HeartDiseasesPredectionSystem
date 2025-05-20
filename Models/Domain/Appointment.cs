using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }

}
