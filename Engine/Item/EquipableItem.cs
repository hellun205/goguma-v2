using System;
using System.Windows.Media;

namespace goguma_v2.Engine.Item;

public class EquipableItem : Item
{
  public override string Type => ItemType.EquipableItem;
  public ushort Count { get; private set; } = 0;

  public override void OnUse()
  {
    throw new System.NotImplementedException();
  }

  public EquipableItem(string code) : base(code)
  {
    
  }


}