using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class ProgressTracking
    {
        [Key]
        public int ProgressId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [ForeignKey("WearableReading")]
        public int ReadingId { get; set; }

        [Required, StringLength(500)]
        public string ProgressNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual WearableReading WearableReading { get; set; }
        public virtual Doctor Doctor { get; set; }
    }


}
