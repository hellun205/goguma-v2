using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Hat : EquipableItem
{
  public override string EType => ItemType.EquipableItemType.Hat;
  
  public HatBuffStats BuffStats { get; set; }
  
  public Hat(string code) : base(code)
  {
    
  }

}