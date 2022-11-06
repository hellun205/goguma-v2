using System;

namespace goguma_v2.Engine.Item.Consumable;

[Serializable]
public abstract class Consumableitem : Item
{
  public override string Type => ItemType.ConsumableItem;
  
  public abstract string CType { get; }

  public abstract void OnUse();

  public Consumableitem(string code) : base(code)
  {
    
  }


}