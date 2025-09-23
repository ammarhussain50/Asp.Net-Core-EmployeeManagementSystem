using EMS_Backend.Model;

namespace EMS_Backend.Interface
{
    public interface ITokenService
    {
       Task <string> CreateTokenAsync(AppUser user);
    }
}
