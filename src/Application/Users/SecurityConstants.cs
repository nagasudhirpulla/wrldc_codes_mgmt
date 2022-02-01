namespace Application.Users;

public static class SecurityConstants
{
    public const string StakeholderRoleString = "Stakeholder";
    public const string AdminRoleString = "Administrator";
    public const string RldcRoleString = "RLDC";
    public static List<string> GetRoles()
    {
        return typeof(SecurityConstants).GetFields().Select(x => x.GetValue(null).ToString()).ToList();
    }
}
