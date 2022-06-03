namespace App.Global.Permissions;

public static class GlobalPermissions
{
    public const string GroupName = "Global";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

}

public static class EmailPermissions
{
    public const string GroupName = "EmailManager";

    public const string Service_Email = GroupName + ".EmailService";
    public const string TemplateDefault = GroupName + ".Template";
    public const string TemplateCreate = TemplateDefault + ".Create";
    public const string TemplateEdit = TemplateDefault + ".Edit";
    public const string TemplateDelete = TemplateDefault + ".Delete";
}
