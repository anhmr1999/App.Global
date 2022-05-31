using App.Global.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using static App.Global.Permissions.GlobalPermissions;

namespace App.Global.Permissions;

public class GlobalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GlobalPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(GlobalPermissions.MyPermission1, L("Permission:MyPermission1"));

        var EmailServicePermission = myGroup.AddPermission(GlobalEmailService.Email, L($"Permission.GlobalEmailService"));
        var EmailConfigPermission = myGroup.AddPermission(GlobalEmailService.Config, L($"Permission.GlobalEmailConfig"));
        var EmailTemplatePermission = myGroup.AddPermission(GlobalEmailTemplate.Default, L("Permission.GlobalEmailTemplate"))
            .AddChild(GlobalEmailTemplate.Create, L("Permission.GlobalEmailTemplate.Create"))
            .AddChild(GlobalEmailTemplate.Edit, L("Permission.GlobalEmailTemplate.Edit"))
            .AddChild(GlobalEmailTemplate.Delete, L("Permission.GlobalEmailTemplate.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GlobalResource>(name);
    }
}
