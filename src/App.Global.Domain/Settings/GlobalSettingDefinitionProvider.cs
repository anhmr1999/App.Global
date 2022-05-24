using Volo.Abp.Settings;

namespace App.Global.Settings;

public class GlobalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(GlobalSettings.MySetting1));
    }
}
