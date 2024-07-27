using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

public class Weapon
{
    public string Model { get; set; } = "";
    public string Team { get; set; } = "";
}

public class Config : BasePluginConfig
{
    [JsonPropertyName("Weapons")]
    public Dictionary<string, Weapon> Weapons { get; set; } = new()
    {
        { "weapon_knife", new Weapon { Model = "", Team = "CT" } },
        { "weapon_ak47", new Weapon { Model = "", Team = "T" } },
        { "weapon_awp", new Weapon { Model = "", Team = "" } },
    };
}