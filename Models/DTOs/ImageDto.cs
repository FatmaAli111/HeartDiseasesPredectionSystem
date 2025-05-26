using Microsoft.AspNetCore.Http;

namespace Models.DTOs
{
    public partial class DoctorProfileDto
    {
        public class ImageDto
        {
            public IFormFile File { get; set; }
        }
    }
}
