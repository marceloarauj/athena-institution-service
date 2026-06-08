namespace Institution.Application.Auth
{
    public interface IUser
    {
        Guid UserId { get; }
        IEnumerable<string> Permissions { get; }
        bool HasPermission(string permissionCode);
    }
}
