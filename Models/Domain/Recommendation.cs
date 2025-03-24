using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }

        [ForeignKey("Diagnosis")]
        public int DiagnosisId { get; set; }

        [Required, StringLength(300)]
        public string Advice { get; set; }

        public DateTime FollowUpDate { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        // Navigation Properties
        public virtual Diagnosis Diagnosis { get; set; }
        public virtual Doctor Doctor { get; set; }
    }


}
