using System;

namespace GogumaV2.Engine.Item.Consumable;

[Serializable]
public sealed class Potion : Item, IConsumable
{
  public string ComsumptionType => ItemType.ConsumptionType.Potion;
  public BuffStats Buff { get; set; }
  public event IConsumable._OnUse? OnUse;
  
  public Potion(string code) : base(code)
  {
    
  }
  
  public void Use()
  {
    Main.player.Stats.Hp = (uint)(Main.player.Stats.Hp + Buff.Hp);
    Main.player.Stats.Moisture = (uint)(Main.player.Stats.Moisture + Buff.Moisture);
    if (Buff.Exp != 0) Main.player.Stats.GainExp(Buff.Exp);
    
    OnUse.Invoke(this);
  }
}