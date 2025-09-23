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
                UserName = dto.Email, // 👈 yahan UserName me email dal diya
                //JobTitle = dto.JobTitle // 👈 naya field map
            };
        }


        public static AuthTokenDto ToNewUserDto(this AppUser user, string token)
        {
            return new AuthTokenDto
            {

                Email = user.Email,
                Token = token,
                //JobTitle = user.JobTitle // 👈 response me bhi bhej do
            };
        }

        public static LoginTokenDto ToLoginUserDto(this AppUser user, string token)
        {
            return new LoginTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = token,
                //JobTitle = user.JobTitle // 👈 login response me bhi
            };
        }

    }
}
