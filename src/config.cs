using CounterStrikeSharp.API.Core;

public class Weapon
{
    public string Model { get; set; } = "";
    public string Permission { get; set; } = "";
    public string Team { get; set; } = "";
}

public class Config : BasePluginConfig
{
    public Dictionary<string, Weapon> Weapons { get; set; } = new()
    {
        { "weapon_knife", new Weapon { Model = "models/example/file.vmdl" } },
        { "weapon_awp", new Weapon { Model = "models/example/file.vmdl", Permission = "@css/reservation", Team = "T" } }
    };
}