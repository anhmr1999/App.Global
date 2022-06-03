using System.Threading.Tasks;
using App.Global.Localization;
using App.Global.MultiTenancy;
using App.Global.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace App.Global.Web.Menus;

public class GlobalMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
            await ConfigureMainMenuAsync(context);
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<GlobalResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                GlobalMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );
        await GetEmailMenuItem(context);

        //remove identity manager
        //context.Menu.GetAdministration().TryRemoveMenuItem(IdentityMenuNames.GroupName);
        context.Menu.GetAdministration().TryRemoveMenuItem(SettingManagementMenuNames.GroupName);
        context.Menu.GetAdministration().TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
    }

    private async Task GetEmailMenuItem(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<GlobalResource>();
        ApplicationMenuItem emailMenuItem = new ApplicationMenuItem(
                GlobalMenus.Email,
                l["Menu:Email"]
            );
        if (await context.IsGrantedAsync(EmailPermissions.Service_Email))
            emailMenuItem.AddItem(
                new ApplicationMenuItem(
                    GlobalMenus.Email,
                    l["Menu:Email"],
                    url: "/Mails"
                )
            );
        if (await context.IsGrantedAsync(EmailPermissions.TemplateDefault))
            emailMenuItem.AddItem(
            new ApplicationMenuItem(
                GlobalMenus.EmailTemplate,
                l["Menu:EmailTemplate"],
                url: "/Mails/EmailTemplate"
                )
            );
        context.Menu.AddItem(emailMenuItem);
    }
}
