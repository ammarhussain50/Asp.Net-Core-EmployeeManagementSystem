using System.ComponentModel.DataAnnotations;

namespace EMS_Backend.DTO
{
    public class AuthDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        //[Required]
        //public string JobTitle { get; set; }  // 👈 naya field
    }

   
}
