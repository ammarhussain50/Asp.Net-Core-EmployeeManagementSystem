namespace EMS_Backend.DTO
{
    public class AuthTokenDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        //public string JobTitle { get; set; } // 👈 new field
    }
}
