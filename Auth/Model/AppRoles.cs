namespace SupportAPI.Auth.Model
{
    public static class AppRoles
    {
        public const string Admin = nameof(Admin);
        public const string RegularUser = nameof(RegularUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, RegularUser };
    }
}
