using Models.Domain;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos.IRepos
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorByIdAsync(int id);
        IEnumerable<AvailableSlotDto> GetAvailability(string doctorId);
        void SetAvailability(string doctorId, List<AvailableSlotDto> slots);
    }
}
