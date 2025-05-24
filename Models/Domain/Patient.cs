using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required, StringLength(10)]
        public string Gender { get; set; }

        // Navigation Properties
        public List<Appointment> Appointments { get; set; }

        public virtual List<MedicalHistory> MedicalHistories { get; set; }
        public virtual List<Diagnosis> Diagnoses { get; set; }
        public virtual List<WearableReading> WearableReadings { get; set; }
        public virtual List<PatientMedication> PatientMedications { get; set; }
    }

}
