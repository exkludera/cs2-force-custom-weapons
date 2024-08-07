using CounterStrikeSharp.API.Core;
using static CounterStrikeSharp.API.Core.Listeners;

namespace ForceCustomWeapons;

public partial class Plugin : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Force Custom Weapons";
    public override string ModuleVersion => "1.0.1";
    public override string ModuleAuthor => "exkludera";

    public override void Load(bool hotReload)
    {
        if (CoreConfig.FollowCS2ServerGuidelines)
            throw new Exception($"Cannot set or get 'CEconEntity::m_OriginalOwnerXuidLow' with \"FollowCS2ServerGuidelines\" option enabled.");

        RegisterListener<OnServerPrecacheResources>(OnServerPrecacheResources);
        RegisterListener<OnEntityCreated>(OnEntityCreated);

        RegisterEventHandler<EventItemEquip>(OnItemEquip);
    }

    public override void Unload(bool hotReload)
    {
        RemoveListener<OnServerPrecacheResources>(OnServerPrecacheResources);
        RemoveListener<OnEntityCreated>(OnEntityCreated);

        DeregisterEventHandler<EventItemEquip>(OnItemEquip);
    }
}