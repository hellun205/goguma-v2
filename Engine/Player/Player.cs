using System;
using goguma_v2.Engine.Item;

namespace goguma_v2.Engine.Player;

[Serializable]
public sealed partial class Player
{
  public string Name { get; private set; }
  public string Class { get; private set; } = "없음";
  public Inventory Inventory { get; set; }
  public Equipment Equipment { get; set; }
  public Stats Stats { get; set; }


  public Player(string name)
  {
    Name = name;
    Inventory = new(new[] {ItemType.EquipableItem, ItemType.ConsumableItem, ItemType.OtherItem});
    Equipment = new(new[]
    {
      ItemType.EquipmentType.Hat, ItemType.EquipmentType.Top, ItemType.EquipmentType.Bottom, ItemType.EquipmentType.Shoes,
      ItemType.EquipmentType.Weapon
    }, Inventory);
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