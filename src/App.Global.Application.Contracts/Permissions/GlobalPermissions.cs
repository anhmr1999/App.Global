namespace App.Global.Permissions;

public static class GlobalPermissions
{
    public const string GroupName = "Global";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class GlobalEmailService
    {
        public const string Email = GroupName + ".EmailService";
        public const string Config = GroupName + ".Config";
    }
    public static class GlobalEmailTemplate
    {
        public const string Default = GroupName + ".Template";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
