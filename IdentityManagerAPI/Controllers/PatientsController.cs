using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs;

namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ApplicationDbContext _context { get; }

        public PatientsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env ,ApplicationDbContext context)
        {
            _userManager = userManager;
            _env = env;
            _context = context;
        }


        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var users = await _userManager.GetUsersInRoleAsync("Patient");

            var userIds = users.Select(u => u.Id).ToList();

            var healthInfos = _context.UserHealthInfos
                .Where(h => userIds.Contains(h.UserId))
                .ToList();

            var patients = users.Select(user =>
            {
                var healthInfo = healthInfos.FirstOrDefault(h => h.UserId == user.Id);

                // 👇 تحويل الصورة إلى Base64
                string imageBase64 = healthInfo?.Image != null
                    ? Convert.ToBase64String(healthInfo.Image)
                    : null;

                return new
                {
                    Id = user.Id,
                    Name = $"{user.Name}",
                    Age = healthInfo?.Age ?? 0,
                    Gender = healthInfo?.Gender ?? "Unknown",
                    MedicalConditions = healthInfo?.MedicalConditions ?? "Unknown",
                    ImageBase64 = imageBase64 // 👈 عرض الصورة
                };
            }).ToList();

            return Ok(patients);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }
    }

}
