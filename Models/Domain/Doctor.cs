using System.ComponentModel.DataAnnotations;

namespace Models.Domain
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required, StringLength(10)]
        public string Gender { get; set; }
        public string Specialty { get; set; }


        [StringLength(200)]
        public string Address { get; set; }

        // Navigation Properties
        public List<Appointment> Appointments { get; set; }

        public virtual List<Recommendation> Recommendations { get; set; }
        public virtual List<ProgressTracking> ProgressTrackings { get; set; }
    }


}
