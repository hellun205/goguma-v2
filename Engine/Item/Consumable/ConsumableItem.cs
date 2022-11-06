using System;
using System.Windows.Media;

namespace goguma_v2.Engine.Item.Consumable;

[Serializable]
public abstract class Consumableitem : Item
{
  public override string Type => ItemType.ConsumableItem;

  public override void OnUse()
  {
    throw new System.NotImplementedException();
  }

  public Consumableitem(string code) : base(code)
  {
    
  }


}