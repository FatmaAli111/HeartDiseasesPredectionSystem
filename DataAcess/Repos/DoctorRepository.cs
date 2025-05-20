using DataAcess.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs;

namespace DataAcess.Repos
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == id);
        }

        public IEnumerable<AvailableSlotDto> GetAvailability(string doctorId)
        {
            return _context.AvailableSlots
                .Where(s => s.DoctorId == doctorId)
                .Select(s => new AvailableSlotDto
                {
                    Date = s.Date,
                    Time = s.Time
                });
        }

        public void SetAvailability(string doctorId, List<AvailableSlotDto> slots)
        {
            var existing = _context.AvailableSlots.Where(s => s.DoctorId == doctorId);
            _context.AvailableSlots.RemoveRange(existing);

            var newSlots = slots.Select(s => new AvailableSlot
            {
                DoctorId = doctorId,
                Date = s.Date,
                Time = s.Time
            });

            _context.AvailableSlots.AddRange(newSlots);
            _context.SaveChanges();
        }
    }
}
