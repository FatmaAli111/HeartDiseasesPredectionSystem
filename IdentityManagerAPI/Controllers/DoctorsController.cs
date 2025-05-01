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

        public DoctorsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var users = await _userManager.GetUsersInRoleAsync("Doctor");

            var doctors = users.Select(user => new
            {
                Id = user.Id,
                Name = user.UserName, // أو FullName لو عامل خاصية كده
                ImageUrl = user.Image // ?? "https://via.placeholder.com/100" // لو مفيش صورة
            }).ToList();

            return Ok(doctors);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> UpdateDoctorProfile([FromForm] DoctorProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            // نملأ بيانات الدكتور
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Specialty = model.Specialty;
            user.YearsOfExperience = model.YearsOfExperience;
            user.Description = model.Description;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;

            // معالجة الصورة
            if (model.Image?.File != null && model.Image.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.File.CopyToAsync(fileStream);
                }

                // نحفظ بيانات الصورة
                user.Image = new Image
                {
                    FileName = fileName,
                    FileExtension = Path.GetExtension(fileName),
                    FileSize = model.Image.File.Length,
                    FilePath = $"/Uploads/{fileName}"
                };
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Done");
        }

    }
}
