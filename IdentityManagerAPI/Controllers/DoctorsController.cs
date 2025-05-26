using DataAcess;
using DataAcess.Repos;
using DataAcess.Repos.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs;
using System.Security.Claims;

namespace IdentityManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env; // محتاجينه عشان نعرف نحفظ الصورة

        public IDoctorRepository _doctorRepo { get; }

        public DoctorsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env,IDoctorRepository DrRepo)
        {
            _userManager = userManager;
            _env = env;
            _doctorRepo = DrRepo;
        }

        [HttpGet]
        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            int pageNumber = 1, pageSize = 10;
            var users = await _userManager.GetUsersInRoleAsync("Doctor");

            var totalDoctors = users.Count;
            var totalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);

            var doctors = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Image = user.Image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.Image)}" : null,
                    Speciality = user.Specialty,
                    YearsOfEx = user.YearsOfExperience,
                    Description = user.Description,
                    Address = user.Address
                })
                .ToList();

            return Ok(new
            {
                TotalCount = totalDoctors,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Doctors = doctors
            });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> UpdateDoctorProfile([FromForm] DoctorProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // تحديث البيانات
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Specialty = model.Specialty;
            user.YearsOfExperience = model.YearsOfExperience;
            user.Description = model.Description;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;

            // لو فيه صورة
            if (model.Image?.File != null && model.Image.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.Image.File.CopyToAsync(memoryStream);
                    user.Image = memoryStream.ToArray(); // خزن الصورة مباشرة في قاعدة البيانات
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Profile updated successfully.");
        }


        [HttpGet("{id}/availability")]
        public IActionResult GetAvailability(string id)
        {
            var availability = _doctorRepo.GetAvailability(id); // should fetch from DB
            return Ok(availability);
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorInfo(int id)
        {
            var doctor = await _doctorRepo.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            return Ok(new
            {
                id = doctor.DoctorId,
                name = doctor.Name,
                specialty = doctor.Specialty
            });
        }

        [HttpPost("{id}/setAvailability")]
        public IActionResult SetAvailability(string id, [FromBody] List<AvailableSlotDto> slots)
        {
            _doctorRepo.SetAvailability(id, slots); // Save to DB
            return Ok(new { success = true });
        }

    }
}
