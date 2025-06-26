using DataAcess.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos
{
    public class AppointmentRepository : Repository<AppointmentRepository>, IAppointment
    {
        private readonly ApplicationDbContext _db;

        public AppointmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Appointment>> GetById(string id)
        {
            var userId = Guid.Parse(id);
            var AppointmentOfUser = await _db.Appointments
                .Where(a => a.UserId == userId && a.Date > DateTime.UtcNow)
                .OrderBy(a => a.Date)
                .ToListAsync();

            return AppointmentOfUser;
        }


    }
}
