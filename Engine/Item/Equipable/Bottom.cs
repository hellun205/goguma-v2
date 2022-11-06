using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public class Bottom : EquipableItem
{
  public override string EquipmentType => ItemType.EquipmentType.Hat;
  public TopBotBuffStats BuffStats { get; set; }

  public Bottom(string code) : base(code)
  {
  }
}