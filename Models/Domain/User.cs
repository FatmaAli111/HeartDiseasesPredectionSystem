using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } // Doctor / Patient
    }

}
