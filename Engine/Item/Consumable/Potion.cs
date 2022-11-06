using System;

namespace goguma_v2.Engine.Item.Consumable;

[Serializable]
public class Potion : Consumableitem
{
  public override string CType => ItemType.ConsumableItemType.Potion;
  
  public PotionBuffStats BuffStats { get; set; }
  
  public Potion(string code) : base(code)
  {
    
  }
  
  public override void OnUse()
  {
    Main.Player.Stats.Hp = (uint)(Main.Player.Stats.Hp + BuffStats.Hp);
    Main.Player.Stats.Moisture = (uint)(Main.Player.Stats.Moisture + BuffStats.Moisture);
    if (BuffStats.Exp != 0) Main.Player.Stats.GainExp(BuffStats.Exp);
  }
}