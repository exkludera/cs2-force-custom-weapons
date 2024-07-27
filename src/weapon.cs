using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using System.Runtime.InteropServices;

namespace CustomWeapons;

public partial class CustomWeapons : BasePlugin, IPluginConfig<Config>
{
    public void OnEntityCreated(CEntityInstance entity)
    {
        if (entity == null)
            return;

        if (!entity.DesignerName.StartsWith("weapon_"))
            return;

        var weapon = entity.As<CBasePlayerWeapon>();

        if (weapon == null)
            return;
        Server.NextWorldUpdate(() =>
        {
            if (weapon.OriginalOwnerXuidLow == 0)
                return;

            var player = Utilities.GetPlayerFromSteamId(weapon.OriginalOwnerXuidLow);

            if (player == null)
                return;

            var activeweapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;

            foreach (var weaponKey in Config.Weapons.Keys)
            {
                if (entity.DesignerName.Contains(weaponKey))
                {
                    string team = Config.Weapons[weaponKey].Team.ToLower();
                    bool isTeamValid = (team == "t" || team == "terrorist") && player.Team == CsTeam.Terrorist ||
                                       (team == "ct" || team == "counterterrorist") && player.Team == CsTeam.CounterTerrorist ||
                                       (team == "" || team == "both") && (player.Team == CsTeam.Terrorist || player.Team == CsTeam.CounterTerrorist);

                    if (!isTeamValid)
                        continue;

                    if (activeweapon != null && weapon == activeweapon)
                        Weapon.UpdateModel(player, activeweapon, Config.Weapons[weaponKey].Model, true);
                    else
                        Weapon.UpdateModel(player, weapon, Config.Weapons[weaponKey].Model, false);

                    break;
                }
            }
        });
    }

    public HookResult OnItemEquip(EventItemEquip @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if (player == null)
            return HookResult.Continue;

        var activeweapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;

        if (activeweapon == null)
            return HookResult.Continue;

        string globalname = activeweapon.Globalname;

        if (!string.IsNullOrEmpty(globalname))
            Weapon.SetViewModel(player, globalname.Split(',')[1]);

        return HookResult.Continue;
    }
}

public class Weapon
{
    public static unsafe CBaseViewModel ViewModel(CCSPlayerController player)
    {
        CCSPlayer_ViewModelServices viewModelServices = new(player.PlayerPawn.Value!.ViewModelServices!.Handle);

        nint ptr = viewModelServices.Handle + Schema.GetSchemaOffset("CCSPlayer_ViewModelServices", "m_hViewModel");
        Span<nint> viewModels = MemoryMarshal.CreateSpan(ref ptr, 3);

        CHandle<CBaseViewModel> viewModel = new(viewModels[0]);

        return viewModel.Value!;
    }
    public static unsafe string GetViewModel(CCSPlayerController player)
    {
        return ViewModel(player).VMName;
    }
    public static unsafe void SetViewModel(CCSPlayerController player, string model)
    {
        ViewModel(player).SetModel(model);
    }
    public static void UpdateModel(CCSPlayerController player, CBasePlayerWeapon weapon, string model, bool update)
    {
        weapon.Globalname = $"{GetViewModel(player)},{model}";
        weapon.SetModel(model);

        if (update)
            SetViewModel(player, model);
    }
    public static void ResetWeapon(CCSPlayerController player, CBasePlayerWeapon weapon, bool update)
    {
        string globalname = weapon.Globalname;

        if (string.IsNullOrEmpty(globalname))
            return;

        string[] globalnamedata = globalname.Split(',');

        weapon.Globalname = string.Empty;
        weapon.SetModel(globalnamedata[0]);

        if (update)
            SetViewModel(player, globalnamedata[0]);
    }
}