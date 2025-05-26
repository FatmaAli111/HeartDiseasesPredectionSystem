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
        public Guid UserId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        //// Navigation Properties
        //public Patient patient { get; set; }
        //public Doctor Doctor { get; set; }
    }

}
