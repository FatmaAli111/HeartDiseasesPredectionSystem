using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserHealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserHealthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveHealthInfo([FromBody] UserHealthInfoDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var info = new UserHealthInfo
        {
            Age = dto.Age,
            Gender = dto.Gender,
            Weight = dto.Weight,
            Height = dto.Height,
            IsSmoker = dto.IsSmoker,
            MedicalConditions = dto.MedicalConditions,
            StressLevel = dto.StressLevel,
            PhysicalTiredness = dto.PhysicalTiredness,
            MentalExhaustion = dto.MentalExhaustion,
            ExerciseTiredness = dto.ExerciseTiredness,
            Exercise = dto.Exercise,
            ExerciseDays = dto.ExerciseDays,
            EnergyLevel = dto.EnergyLevel,
            Diabetes = dto.Diabetes,
            BloodPressureMedications = dto.BloodPressureMedications,
            HeartRate = dto.HeartRate,
            GlucoseLevel = dto.GlucoseLevel,
            SystolicBP = dto.SystolicBP,
            Cholesterol = dto.Cholesterol,
            Angina = dto.Angina,
            CardiovascularDisease = dto.CardiovascularDisease,
            CoronaryHeartDisease = dto.CoronaryHeartDisease,
            HeartAttack = dto.HeartAttack,
            Stroke = dto.Stroke,
            Hypertension = dto.Hypertension,
            UserId = user.Id
        };

        _context.UserHealthInfos.Add(info);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Health info saved successfully." });
    }

    [HttpPost("predict-and-save")]
    public async Task<IActionResult> PredictAndSave([FromBody] PredictionRequestDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var client = new HttpClient();
        var flaskUrl = "http://127.0.0.1:5000/predict";

        var requestData = new
        {
            gender = dto.Gender,
            age = dto.Age,
            smokes = dto.Smokes,
            cigarettes_per_day = dto.CigarettesPerDay,
            weight_kg = dto.WeightKg,
            height_cm = dto.HeightCm,
            diabetes = dto.Diabetes,
            bp_meds = dto.BpMeds,
            heart_rate = dto.HeartRate,
            glucose = dto.Glucose,
            education = dto.Education,
            cholesterol = dto.Cholesterol
        };

        var response = await client.PostAsJsonAsync(flaskUrl, requestData);

        if (!response.IsSuccessStatusCode)
            return StatusCode(500, "Prediction service failed.");

        var flaskResponse = await response.Content.ReadFromJsonAsync<FlaskResponseDto>();

        // تحويل البيانات لحفظها في جدول UserHealthInfo
        var healthInfo = new UserHealthInfo
        {
            Age = dto.Age,
            Gender = dto.Gender,
            Weight = dto.WeightKg,
            Height = dto.HeightCm,
            IsSmoker = dto.Smokes ? "Yes" : "No",
            Diabetes = dto.Diabetes ? "Yes" : "No",
            BloodPressureMedications = dto.BpMeds ? "Yes" : "No",
            HeartRate = dto.HeartRate,
            GlucoseLevel = dto.Glucose,
            Cholesterol = dto.Cholesterol.ToString(),
            UserId = user.Id,
            SystolicBP = flaskResponse.Predictions.SYSBP.ToString("F2"),
            Hypertension = flaskResponse.MetricsAnalysis["Systolic BP"].Status,
        };

        _context.UserHealthInfos.Add(healthInfo);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Prediction done and health info saved.",
            predictions = flaskResponse.Predictions,
            metrics = flaskResponse.MetricsAnalysis
        });
    }
}
