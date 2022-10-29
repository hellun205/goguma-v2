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
    public ushort Level { get; private set; } = 1;
    public uint Exp { get; private set; } = 0;
    public uint Hp { get; private set; } = 50;
    public uint MaxHp { get; private set; } = 50;
    public string Class { get; private set; } = "없음";

    public static uint GetNextLevelExp(ushort level) => (uint)Math.Floor(0.04 * (level ^ 3) + 0.8 * (level ^ 2) + 2 * level);

    public Player(string name)
    {
      Name = name;
    }

    public void GainExp(uint amount)
    {
      uint leftAmount;
      if (Exp + amount >= GetNextLevelExp(Level))
      {
        leftAmount = Exp + amount - GetNextLevelExp(Level);
        Level += 1;
        GainExp(leftAmount);
      }
      else
      {
        Exp += amount;
      }
    }

    public void GainHp(uint amount) => Hp = Math.Min(MaxHp, Hp + amount);

    public void LoseHp(uint amount)
    {
      if (Hp < amount)
        Hp = 0;
      else
        Hp -= amount;
    }

    public void GainMaxHp(uint amount) => MaxHp += amount;

    public void LoseMaxHp(uint amount)
    {
      if (MaxHp < amount)
      {
        MaxHp = 1;
        Hp = Math.Min(Hp, MaxHp);
      }
      else
        MaxHp -= amount;
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

    public static PlayerData LoadData(string name) => PlayerData.Load($"{SavePath}/{name}.json");
  }
}