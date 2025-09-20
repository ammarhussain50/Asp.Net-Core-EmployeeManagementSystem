using EMS_Backend.DTO;
using EMS_Backend.Model;

namespace EMS_Backend.Mappers
{
    public static class AuthMappers
    {
        public static AppUser ToAppUser(this AuthDto dto)
        {
            return new AppUser
            {

                Email = dto.Email,
                // dont use Password here, it will be set by UserManager in controller because it hased and salted automatically
                UserName = dto.Email // 👈 yahan UserName me email dal diya
            };
        }


        public static AuthTokenDto ToNewUserDto(this AppUser user, string token)
        {
            return new AuthTokenDto
            {

                Email = user.Email,
                Token = token
            };
        }

    }
}
