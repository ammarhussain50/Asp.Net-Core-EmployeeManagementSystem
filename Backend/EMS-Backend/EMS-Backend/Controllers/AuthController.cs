
using EMS_Backend.DTO;
using EMS_Backend.Interface;
using EMS_Backend.Mappers;
using EMS_Backend.Model;
using EMS_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Login([FromBody] AuthDto authDto)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = authDto.ToAppUser();
                var createdUser = await _userManager.CreateAsync(appUser, authDto.Password);

                if (createdUser.Succeeded)
                {
                    //// 👇 JobTitle ko Role banake assign kar rahe hain
                    //if (!string.IsNullOrEmpty(authDto.JobTitle))
                    //{
                    //    // ensure role exists
                    //    if (!await _userManager.IsInRoleAsync(appUser, authDto.JobTitle))
                    //    {
                    //        // role create karo agar nahi hai
                    //        var roleExists = await _userManager.AddToRoleAsync(appUser, authDto.JobTitle);
                    //    }
                    //}
                    await _userManager.AddToRoleAsync(appUser, "Admin"); // "User" role assign kiya

                    var token = await _tokenService.CreateTokenAsync(appUser);
                    var newUserDto = appUser.ToNewUserDto(token);

                    return Ok(newUserDto);
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // Input validation fail ho gayi

                // Email ke zariye user find karo
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                    return Unauthorized("Invalid email or password"); // User exist nahi karta

                // Password check karo Identity system se
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);


                if (!result.Succeeded)
                {
                    return Unauthorized("Invalid email or password"); // Password match nahi hua
                }

                var token = await _tokenService.CreateTokenAsync(user); // Token generate karo

                var userDto = user.ToLoginUserDto(token);    // DTO banayo login response ke liye
                return Ok(userDto);                          // 200 OK with token
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }

        }

    }
}