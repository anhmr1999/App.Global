using App.Global.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using static App.Global.Permissions.GlobalPermissions;

namespace App.Global.Permissions;

public class GlobalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //context.RemoveGroup("AbpIdentity");
        context.RemoveGroup("AbpTenantManagement");
        context.RemoveGroup("FeatureManagement");

        var myGroup = context.AddGroup(GlobalPermissions.GroupName);

        //Email
        var EMailGroup = context.AddGroup(EmailPermissions.GroupName, L($"Permission.GlobalEmailManagement"));
        EMailGroup.AddPermission(EmailPermissions.Service_Email, L($"Permission.GlobalEmailService"));
        var EmailTemplatePermission = EMailGroup.AddPermission(EmailPermissions.TemplateDefault, L("Permission.GlobalEmailTemplate"));
        EmailTemplatePermission.AddChild(EmailPermissions.TemplateCreate, L("Permission.GlobalEmailTemplate.Create"));
        EmailTemplatePermission.AddChild(EmailPermissions.TemplateEdit, L("Permission.GlobalEmailTemplate.Edit"));
        EmailTemplatePermission.AddChild(EmailPermissions.TemplateDelete, L("Permission.GlobalEmailTemplate.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GlobalResource>(name);
    }
}
