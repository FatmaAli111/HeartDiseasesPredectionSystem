using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UserHealthInfoDto
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public string IsSmoker { get; set; }
        public string MedicalConditions { get; set; }
        public string StressLevel { get; set; }
        public string PhysicalTiredness { get; set; }
        public string MentalExhaustion { get; set; }
        public string ExerciseTiredness { get; set; }
        public string Exercise { get; set; }
        public int? ExerciseDays { get; set; }
        public int EnergyLevel { get; set; }
        public string Diabetes { get; set; }
        public string BloodPressureMedications { get; set; }
        public int? HeartRate { get; set; }
        public int? GlucoseLevel { get; set; }
        public string SystolicBP { get; set; }
        public string Cholesterol { get; set; }
        public string Angina { get; set; }
        public string CardiovascularDisease { get; set; }
        public string CoronaryHeartDisease { get; set; }
        public string HeartAttack { get; set; }
        public string Stroke { get; set; }
        public string Hypertension { get; set; }
    }

}
