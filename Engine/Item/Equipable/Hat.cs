using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Hat : EquipableItem
{
  public override string EquipmentType => ItemType.EquipmentType.Hat;
  
  public Hat(string code) : base(code)
  {
    
  }

}