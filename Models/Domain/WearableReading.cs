using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class WearableReading
    {
        [Key]
        public int ReadingId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [Range(30, 200)]
        public int HeartRate { get; set; }

        [Required, StringLength(20)]
        public string BloodPressure { get; set; }

        [Range(50, 100)]
        public int OxygenLevel { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual List<ProgressTracking> ProgressTrackings { get; set; }
    }


}
