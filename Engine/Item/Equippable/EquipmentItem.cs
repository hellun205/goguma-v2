using System;

namespace goguma_v2.Engine.Item.Equippable;

[Serializable]
public sealed class EquipmentItem : Item, IEquippable
{
  public string EquipmentType { get; init; }

  public BuffStats Buff { get; set; }

  public event IEquippable._OnEquip? OnEquip;
  public event IEquippable._OnUnEquip? OnUnEquip;

  public EquipmentItem(string code, string equipmentType) : base(code)
  {
    EquipmentType = equipmentType;
  }

  public void Equip()
  {
    if (OnEquip != null) OnEquip.Invoke(this);
  }

  public void UnEquip()
  {
    if (OnUnEquip != null) OnUnEquip.Invoke(this);
  }
}