using EMS_Backend.DTO;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> userRepo;
        private readonly IConfiguration config;

        public AuthController(IRepository<User> UserRepo, IConfiguration config)
        {
            userRepo = UserRepo;
            this.config = config;
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] AuthDto model)
        //{
        //    var users = await userRepo.GetAllAsync();
        //    var user = users.FirstOrDefault(u => u.Email == model.Email);

        //    if (user == null || user.Password != model.Password)
        //    {
        //        return BadRequest(new { message = "Invalid email or password" });
        //    }

        //    //yahan role ko use karo(agar tumhare User entity me role hai)
        //    //var token = GenerateToken(user.Email, user.Role ?? "User");

        //    return Ok(new AuthTokenDto()
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        Token = token
        //    });
        //}
       

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] AuthDto model)
            {
                var users = await userRepo.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    return BadRequest(new { message = "Invalid email or password" });
                }

                var token = GenerateToken(user.Email, user.Role);

                return Ok(new AuthTokenDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token
                });
            }


        private string GenerateToken(string email, string role)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role ?? "User")
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public string GenerateToken(string email, string role)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4d66225e1be29727a6da08bb9c8f250f8defed81a2c943de828160963f600100"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.Name, email),
        //    new Claim(ClaimTypes.Role, role ?? "User")
        //};

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}



    }
}
