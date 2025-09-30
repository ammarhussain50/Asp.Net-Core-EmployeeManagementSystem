using Microsoft.AspNetCore.Identity;

namespace EMS_Backend.Model
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImage { get; set; }
    }
}
