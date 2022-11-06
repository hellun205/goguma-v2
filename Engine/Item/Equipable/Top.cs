using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Top : EquipableItem
{
  public override string EType => ItemType.EquipableItemType.Top;
  
  public TopBotBuffStats BuffStats { get; set; }
  
  public Top(string code) : base(code)
  {
    
  }

}