using DataAcess.Repos.IRepos;
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

        public async Task<List<Appointment>> GetById(int id)
        {
          var AppointmentOfUser= _db.Appointments.
                Where(a => a.UserId == id && a.Date > DateTime.UtcNow).OrderBy(a=>a.Date).ToList(); 
            
            return  AppointmentOfUser;
        }

      
    }
}
