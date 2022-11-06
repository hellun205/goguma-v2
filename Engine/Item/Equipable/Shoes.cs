using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Shoes : EquipableItem
{
  public override string EType => ItemType.EquipableItemType.Hat;
  
  public ShoesBuffStats BuffStats { get; set; }
  
  public Shoes(string code) : base(code)
  {
    
  }

}