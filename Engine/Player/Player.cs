using System;

namespace goguma_v2.Engine.Player;

[Serializable]
public partial class Player
{
  public string Name { get; private set; }
  public string Class { get; private set; } = "없음";
  public Inventory.Inventory Inventory { get; set; }
  public Stats Stats { get; set; }


  public Player(string name)
  {
    Name = name;
    Inventory = new(new[] {"장비", "소비", "기타"});
    Stats = new Stats()
    {
      MaxHp = 50,
      Hp = 50,
      AvoidanceRate = 0,
      ReceiveExpGrowthRate = 0,
    };
  }


  public PlayerData GetData() => new PlayerData() {Name = this.Name, Class = this.Class, Level = Stats.Level};
}