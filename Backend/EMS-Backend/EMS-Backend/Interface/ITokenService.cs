using EMS_Backend.Model;

namespace EMS_Backend.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
