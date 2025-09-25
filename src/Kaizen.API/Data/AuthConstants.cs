namespace Kaizen.API.Data;

public static class AuthConstants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class Policies
    {
        public const string RequireAdminRole = "RequireAdminRole";
    }
}