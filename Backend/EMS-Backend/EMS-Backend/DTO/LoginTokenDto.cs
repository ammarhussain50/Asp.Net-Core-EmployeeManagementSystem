namespace EMS_Backend.DTO
{
    public class LoginTokenDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
        public string Role { get; set; }
        //public string? JobTitle { get; internal set; }
    }
}
