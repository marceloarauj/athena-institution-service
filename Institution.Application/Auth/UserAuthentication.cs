using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Institution.Application.Auth
{
    public class UserAuthentication(IHttpContextAccessor httpContextAccessor) : IUser
    {
        private ClaimsPrincipal User => httpContextAccessor.HttpContext!.User;

        public Guid UserId => Guid.Parse(User.FindFirst("user_id")!.Value);

        public IEnumerable<string> Permissions =>
            User.Claims.Where(c => c.Type == "permission").Select(c => c.Value);

        public bool HasPermission(string permissionCode) =>
            User.Claims.Any(c => c.Type == "permission" && c.Value == permissionCode);
    }
}
