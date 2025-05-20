using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class AvailableSlot
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public Doctor Doctor { get; set; }
    }
}
