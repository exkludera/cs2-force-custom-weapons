using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace CustomWeapons;

public partial class CustomWeapons : BasePlugin, IPluginConfig<Config>
{
    public Config Config { get; set; } = new Config();
    public void OnConfigParsed(Config config) { Config = config; }

    public void OnServerPrecacheResources(ResourceManifest manifest)
    {
        foreach (var weapon in Config.Weapons.Values)
            manifest.AddResource(weapon.Model);
    }
}