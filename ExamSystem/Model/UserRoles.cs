namespace ExamSystem.Model;

public static class UserRoles
{
    public const string ADMIN = nameof(ADMIN);
    public const string USER = nameof(USER);
    public const string ADMIN_AND_USER = $"{nameof(USER)},{nameof(ADMIN)}";

    public static readonly IReadOnlyCollection<string> ALL = new[] { ADMIN, USER };
}   
