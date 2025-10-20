using EMS_Backend.DTO;
using EMS_Backend.Entity;
using EMS_Backend.Model;

namespace EMS_Backend.Mappers
{
    public static class ProfileMappers
    {
        public static void UpdateUserFromDto(this AppUser user, ProfileDto dto)
        {
            //user.Email = dto.Email;
            //user.UserName = dto.Email; // Identity ka required field
            user.ProfileImage = dto.ProfileImage;
            // Password yahan nahi update karenge, woh UserManager karega
        }

        public static void UpdateEmployeeFromDto(this Employee employee, ProfileDto dto)
        {
            employee.Name = dto.Name;
            //employee.Email = dto.Email;
            employee.Phone = dto.Phone;
        }

        public static ProfileResponseDto ToProfileResponseDto(this( AppUser user, Employee employee) data)
        {
            return new ProfileResponseDto
            {
                Name = data.employee?.Name,
                Phone = data.employee?.Phone,
                ProfileImage = data.user?.ProfileImage,
                Salary = data.employee?.Salary

            };
        }
    }
}
