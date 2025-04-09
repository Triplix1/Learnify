namespace Learnify.Core.Enums;

public enum Role
{
    Student,
    Teacher,
    Moderator,
    Admin,
    SuperAdmin
}

public static class RoleLists
{
    public static List<Role> ModeratorsRoles = new List<Role>()
    {
        Role.Moderator,
        Role.Admin,
        Role.SuperAdmin
    };
}