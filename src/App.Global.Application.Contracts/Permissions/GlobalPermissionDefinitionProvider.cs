using App.Global.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace App.Global.Permissions;

public class GlobalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GlobalPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(GlobalPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GlobalResource>(name);
    }
}
