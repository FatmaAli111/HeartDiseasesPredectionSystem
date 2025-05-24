using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos.IRepos
{
   public interface IAppointment:IRepository<AppointmentRepository> 
    {
        Task<List<Appointment>> GetById(int id); 
    }
}
