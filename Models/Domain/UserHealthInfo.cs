using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public class UserHealthInfo
{
    public int Id { get; set; }

    // كل الحقول اللي عندك زي ما هي
    public int Age { get; set; }
    public string Gender { get; set; } = "Unknown";
    public float? Weight { get; set; }
    public float? Height { get; set; }
    public string IsSmoker { get; set; } = "Unknown";
    public string MedicalConditions { get; set; } = "Unknown";

    public string StressLevel { get; set; } = "Unknown";
    public string PhysicalTiredness { get; set; } = "Unknown";
    public string MentalExhaustion { get; set; } = "Unknown";
    public string ExerciseTiredness { get; set; } = "Unknown";
    public string Exercise { get; set; } = "Unknown";
    public int? ExerciseDays { get; set; }
    public int EnergyLevel { get; set; }

    public string Diabetes { get; set; } = "Unknown";
    public string BloodPressureMedications { get; set; } = "Unknown";
    public int? HeartRate { get; set; }
    public int? GlucoseLevel { get; set; }
    public string SystolicBP { get; set; } = "Unknown";
    public string Cholesterol { get; set; } = "Unknown";
    public string Angina { get; set; } = "Unknown";
    public string CardiovascularDisease { get; set; } = "Unknown";
    public string CoronaryHeartDisease { get; set; } = "Unknown";
    public string HeartAttack { get; set; } = "Unknown";
    public string Stroke { get; set; } = "Unknown";
    public string Hypertension { get; set; } = "Unknown";

    // ✅ أهم خطوة: ربط البيانات باليوزر
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; }
    public byte[]? Image { get; set; }

}
