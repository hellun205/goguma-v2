using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Nodes;

namespace goguma_v2.Engine.Player
{
  [Serializable]
  public class Player
  {
    public static string SavePath => "./Save";
    public string Name { get; private set; }
    public int Level { get; private set; } = 1;
    public int Exp { get; private set; } = 0;
    public int Hp { get; private set; } = 50;
    public int MaxHp { get; private set; } = 50;
    public string Class { get; private set; } = "없음";

    public static int GetNextLevelExp(int level) => (int)Math.Floor(0.04 * (level ^ 3) + 0.8 * (level ^ 2) + 2 * level);

    public Player(string name)
    {
      Name = name;
    }
    
    public PlayerData GetData() => new PlayerData() {Name = this.Name, Class = this.Class, Level = this.Level};


    public static void Save(Player player)
    {
      if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
      
      Stream ws = new FileStream($"{SavePath}/{player.Name}.pdt", FileMode.Create);
      BinaryFormatter serializer = new BinaryFormatter();
      
      serializer.Serialize(ws, player);
      ws.Close();
      
      PlayerData.Save($"{SavePath}/{player.Name}.json", player.GetData());
    }

    public static Player Load(string name)
    {
      Stream rs = new FileStream($"{SavePath}/{name}.pdt", FileMode.Open);
      BinaryFormatter deserializer = new BinaryFormatter();

      Player obj = (Player)deserializer.Deserialize(rs);
      rs.Close();
      return obj;
    }
  }
}