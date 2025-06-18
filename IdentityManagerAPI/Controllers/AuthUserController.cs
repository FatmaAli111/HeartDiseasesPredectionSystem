using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Auth;
using Models;
using System.Net;
using DataAcess.Repos.IRepos;
using Models.Domain;
using IdentityManager.Services.ControllerService.IControllerService;
using Models.DTOs.EmailSender;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.Web;

namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly EmailSettings _emailSettings;

        public UserManager<ApplicationUser> _userManager { get; }

        public AuthUserController(IAuthService authService, UserManager<ApplicationUser> userManager, IOptions<EmailSettings> emailSettings)
        {
            _authService = authService;
            _userManager = userManager;
            _emailSettings = emailSettings.Value;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);

            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var result = await _authService.RegisterAsync(registerRequestDTO);
            return Ok(result);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully. Please remove your token." });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token); // مهم جدًا 

            var resetLink = $"http://127.0.0.1:5500/resetpassword.html?token={encodedToken}&email={email}";


            var fromAddress = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
            var toAddress = new MailAddress(email);
            string subject = "Reset Your Password";
            string body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

            using (var smtp = new SmtpClient
            {
                Host = _emailSettings.SmtpHost,
                Port = _emailSettings.SmtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password)
            })
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
            }

            return Ok(new { message = "Reset link sent. Check your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Password reset successfully." });
        }



    }
}
