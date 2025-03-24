using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class MedicalHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required, StringLength(500)]
        public string Notes { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Patient Patient { get; set; }
    }

}
