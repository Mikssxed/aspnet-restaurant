using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal? User { get; }
        int? GetUserId();
    }
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserId()
        {
            var idClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                return null;
            }

            return int.Parse(idClaim.Value);
        }
    }
}