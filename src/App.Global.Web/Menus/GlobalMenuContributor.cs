using System.Threading.Tasks;
using App.Global.Localization;
using App.Global.MultiTenancy;
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
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
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
        context.Menu.AddItem(
            new ApplicationMenuItem(
                GlobalMenus.Email,
                l["Menu:Email"]
            ).AddItem(
                new ApplicationMenuItem(
                    GlobalMenus.Email,
                    l["Menu:Email"],
                    url: "/Mails"
                )
            ).AddItem(
                new ApplicationMenuItem(
                    GlobalMenus.EmailTemplate,
                    l["Menu:EmailTemplate"],
                    url: "/Mails/EmailTemplate"
                )
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
    }
}
