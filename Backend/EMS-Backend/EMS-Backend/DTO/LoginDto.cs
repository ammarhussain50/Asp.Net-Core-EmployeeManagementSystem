using System.ComponentModel.DataAnnotations;

namespace EMS_Backend.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        // 👇 JobTitle ni hoga login ke time
    }
}
