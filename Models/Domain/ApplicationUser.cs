using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public int? ImageId { get; set; }
        
        // Navigation property
        [ForeignKey("ImageId")]
        public byte[] Image { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public string? Address { get; set; }

    }
}
