using System;

namespace goguma_v2.Engine.Item.Equippable;

[Serializable]
public sealed class EquipmentItem : Item, IEquippable
{
  public string EquipmentType { get; init; }

  public BuffStats Buff { get; set; }

  public event IEquippable._OnEquip? OnEquip;

  public EquipmentItem(string code, string equipmentType) : base(code)
  {
    EquipmentType = equipmentType;
  }

  public void Equip(Player.Player player)
  {
    OnEquip.Invoke(this);
    throw new NotImplementedException();
  }
}