using DataAcess;
using DataAcess.Repos;
using DataAcess.Repos.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs;
using System.Security.Claims;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointment _appointment;

    public ApplicationDbContext _context { get; }

    public AppointmentsController(IAppointment appointment, ApplicationDbContext context)
    {
        _appointment = appointment;
        _context = context;
    }

    [HttpGet("upcoming/{userId}")]
    public IActionResult GetUpcomingAppointments(Guid userId)
    {

        var appointments = _appointment.GetById(userId);
        return Ok(appointments);
    }

    [HttpPost("schedule")]
    [Authorize] 
    public IActionResult ScheduleAppointment([FromBody] AppointmentDto dto)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid user.");
        }

        bool isBooked = _context.Appointments.Any(a =>
            a.DoctorId == dto.DoctorId &&
            a.Date.Date == dto.Date.Date &&
            a.Time == dto.Time
        );

        if (isBooked)
        {
            return BadRequest("This time slot is already booked.");
        }

        var appointment = new Appointment
        {
            UserId = Guid.Parse(userId), 
            DoctorId = dto.DoctorId,
            Date = dto.Date,
            Time = dto.Time
        };

        _context.Appointments.Add(appointment);
        _context.SaveChanges();

        return Ok(new { success = true, appointmentId = appointment.Id });
    }

    [HttpPut("update")]
    public IActionResult UpdateAppointment([FromBody] AppointmentDto dto)
    {
        // Update appointment in DB
        return Ok(new { success = true });
    }

    [HttpDelete("delete")]
    public IActionResult DeleteAppointment(string userId, string doctorId, DateTime date, string time)
    {
        // Delete appointment from DB
        return Ok(new { success = true });
    }

   // [HttpGet("availableSlots")]
    //public IActionResult GetAvailableSlots(string doctorId, DateTime date)
    //{
    //    // Example: return available slots dynamically from DB based on doctorId and date
    //    var allSlots = new[] { "10:00 AM", "11:00 AM", "12:00 PM", "2:00 PM", "3:00 PM" };
    //    //var bookedSlots = GetBookedSlotsFromDatabase(doctorId, date);

    //    var availableSlots = allSlots.Except().ToArray();
    //    return Ok(availableSlots);
    //}


    
}