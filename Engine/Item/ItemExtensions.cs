namespace Goguma.Engine.Item;

public static class ItemExtensions
{
  public static string GetDisplay(this Item item)
  {
    switch (item)
    {
      case IConsumable consumableItem: return $"[ {consumableItem.ComsumptionType} ] {item.Name}";
      case IEquippable equippableItem: return $"[ {equippableItem.EquipmentType} ] {item.Name}";
      default: return item.Name;
    }
  }
}