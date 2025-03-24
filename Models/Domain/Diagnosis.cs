using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Diagnosis
    {
        [Key]
        public int DiagnosisId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required, StringLength(100)]
        public string Result { get; set; }

        [StringLength(500)]
        public string Explanation { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual List<Recommendation> Recommendations { get; set; }
    }

}
