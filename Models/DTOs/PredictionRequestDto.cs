using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PredictionRequestDto
    {
        public string Gender { get; set; } // "male" أو "female"
        public int Age { get; set; }
        public bool Smokes { get; set; }
        public int CigarettesPerDay { get; set; }
        public float WeightKg { get; set; }
        public float HeightCm { get; set; }
        public bool Diabetes { get; set; }
        public bool BpMeds { get; set; }
        public int HeartRate { get; set; }
        public int Glucose { get; set; }
        public string Education { get; set; }
        public int Cholesterol { get; set; }
    }

}
