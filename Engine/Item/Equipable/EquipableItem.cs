using System;
using System.Windows.Media;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public abstract class EquipableItem : Item
{
  public override string Type => ItemType.EquipableItem;
  
  public abstract string EquipmentType { get; }
  
  public BuffStats BuffStats { get; }

  public override void OnUse()
  {
    throw new System.NotImplementedException();
  }

  public EquipableItem(string code) : base(code)
  {
    
  }


}