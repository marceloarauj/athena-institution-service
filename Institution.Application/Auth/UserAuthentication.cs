using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Institution.Application.Auth
{
    public class UserAuthentication(IHttpContextAccessor _httpContextAccessor) : IUser
    {
        //public ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        //public Guid UserId => Guid.Parse(User.FindFirst("user_id")!.Value);
        public Guid UserId => Guid.NewGuid();
    }
}
