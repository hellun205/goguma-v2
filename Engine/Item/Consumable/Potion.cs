using System;

namespace goguma_v2.Engine.Item.Consumable;

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
    Main.Player.Stats.Hp = (uint)(Main.Player.Stats.Hp + Buff.Hp);
    Main.Player.Stats.Moisture = (uint)(Main.Player.Stats.Moisture + Buff.Moisture);
    if (Buff.Exp != 0) Main.Player.Stats.GainExp(Buff.Exp);
    
    OnUse.Invoke(this);
  }
}