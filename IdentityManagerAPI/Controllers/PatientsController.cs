using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public PatientsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var users = await _userManager.GetUsersInRoleAsync("Patient");

            var patients = users.Select(user => new
            {
               // Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                
               // Image = user.Image != null ? user.Image.FilePath : "https://via.placeholder.com/100"
            }).ToList();

            return Ok(patients);
        }
    }
}
