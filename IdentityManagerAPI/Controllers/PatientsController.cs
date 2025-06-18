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

            // هاتي الـ IDs الخاصة بالمستخدمين
            var userIds = users.Select(u => u.Id).ToList();

            // هاتي الـ UserHealthInfo المرتبط بالمستخدمين دول
            var healthInfos = _context.UserHealthInfos
                .Where(h => userIds.Contains(h.UserId))
                .ToList();

            // دمج البيانات
            var patients = users.Select(user =>
            {
                var healthInfo = healthInfos.FirstOrDefault(h => h.UserId == user.Id);

                return new
                {
                    Id = user.Id,
                    Name = $"{user.UserName}", // ممكن تخليها FirstName + LastName لو موجودين
                    Age = healthInfo?.Age ?? 0,
                    Gender = healthInfo?.Gender ?? "Unknown",
                    MedicalConditions = healthInfo?.MedicalConditions ?? "Unknown"
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
