using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class PatientMedication
    {
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [ForeignKey("Medication")]
        public int MedicationId { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual Medication Medication { get; set; }
    }


}
