namespace goguma_v2.Engine.Item;

public interface IEquippable
{
  public delegate void _OnEquip(object sender);

  public delegate void _OnUnEquip(object sender);
  
  public string EquipmentType { get; init; }
  
  public BuffStats Buff { get; set; }

  public event _OnEquip OnEquip;

  public event _OnUnEquip OnUnEquip;

  public void Equip();

  public void UnEquip();
}