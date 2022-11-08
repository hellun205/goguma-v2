using System;
using System.Collections.Generic;
using goguma_v2.Engine.Item;

namespace goguma_v2.Engine.Player;

[Serializable]
public sealed class Equipment
{
  /// <summary>
  /// Key: Type of Equipment, Value: Code of Item 
  /// </summary>
  public Dictionary<string, string> Items { get; private set; }

  private Inventory inventory;

  public Equipment(string[] typeOfEquipment, Inventory inventory)
  {
    Items = new Dictionary<string, string>();
    this.inventory = inventory;

    foreach (string type in typeOfEquipment)
      Items.Add(type, Item.Item.Empty);
  }

  public void Open()
  {
    throw new NotImplementedException();
  }

  public void EquipItem(string itemCode)
  {
    var item = Item.Item.Get(itemCode);
    if (inventory.CheckItem(itemCode) && item is IEquippable)
    {
      string type = ((IEquippable) item).EquipmentType;
      inventory.LoseItem(itemCode, 1);
      Items[type] = itemCode;
    }
  }

  public void UnEquipItem(string typeOfEquipment)
  {
    throw new NotImplementedException();
  }
}