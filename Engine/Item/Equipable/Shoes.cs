using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Shoes : EquipableItem
{
  public override string EquipmentType => ItemType.EquipmentType.Hat;
  
  public Shoes(string code) : base(code)
  {
    
  }

}