using System.Text.Json.Serialization;

namespace PKHeX.MonFS;

public class StringsMon
{
    [JsonPropertyName("species")]
    public string Species { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("held_item")]
    public string Item { get; set; }

    [JsonPropertyName("ribbons")]
    public byte Ribbons { get; set; }

    [JsonPropertyName("ot_tid")]
    public ushort OtTid { get; set; }

    [JsonPropertyName("shiny")]
    public bool Shiny { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("pc_mark")]
    public List<bool> PcMark { get; set; }

    [JsonPropertyName("ball")]
    public string CapturedBall { get; set; }

    [JsonPropertyName("move_set")]
    public List<string> MoveSet { get; set; }

    [JsonPropertyName("ot_name")]
    public string OtName { get; set; }

    [JsonPropertyName("exp")]
    public uint Exp { get; set; }

    [JsonPropertyName("met_level")]
    public byte MetLevel { get; set; }

    [JsonPropertyName("virus")]
    public bool Virus { get; set; }

    [JsonPropertyName("ot_gender")]
    public bool OtGender { get; set; }

}

public class PC
{
    [JsonPropertyName("mons")]
    public List<StringsMon> Mons { get; set; }

    public PC()
    {
        Mons = new List<StringsMon>();
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(PC))]
public partial class PcJsonContext : JsonSerializerContext
{
}
