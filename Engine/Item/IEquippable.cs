namespace goguma_v2.Engine.Item;

public interface IEquippable
{
  public delegate void _OnEquip(object sender);
  
  public string EquipmentType { get; init; }
  
  public BuffStats Buff { get; set; }

  public event _OnEquip OnEquip;

  public void Equip(Player.Player player);
}