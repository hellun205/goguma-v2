using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public abstract class EquipableItem : Item
{
  public override string Type => ItemType.EquipableItem;
  
  public abstract string EType { get; }
  
  public BuffStats BuffStats { get; }

  public EquipableItem(string code) : base(code)
  {
    
  }


}