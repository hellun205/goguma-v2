using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GogumaV2.Engine.Player;

public sealed class PlayerData
{
  [JsonProperty("name")] public string Name { get; set; }

  [JsonProperty("class")] public string Class { get; set; }

  [JsonProperty("lv")] public ushort Level { get; set; }

  public static PlayerData Load(string path)
  {
    using (StreamReader file = File.OpenText(path))
    using (JsonTextReader reader = new JsonTextReader(file))
    {
      JObject json = (JObject) JToken.ReadFrom(reader);
      return JsonConvert.DeserializeObject<PlayerData>(json.ToString());
    }
  }

  public static void Save(string path, PlayerData playerData)
  {
    JObject json = JObject.FromObject(playerData);
    File.WriteAllText(path, json.ToString());
  }

  public override string ToString() => $"{Name} ( Lv. {Level} / {Class} )";
}