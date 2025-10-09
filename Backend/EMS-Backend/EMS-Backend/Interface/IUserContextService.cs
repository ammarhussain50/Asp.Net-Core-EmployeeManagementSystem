using System.Security.Claims;

namespace EMS_Backend.Interface
{
    public interface IUserContextService
    {
        Task<int?> GetEmployeeIdFromClaimsAsync(ClaimsPrincipal user);
    }
}
