using System;
using System.Collections.Generic;
using System.Linq;
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
    if (inventory.CheckItem(itemCode))
    {
      if (item is IEquippable)
      {
        string type = ((IEquippable) item).EquipmentType;
        if (Items[type] != Item.Item.Empty)
          UnEquipItem(type);
        inventory.LoseItem(itemCode, 1);
        Items[type] = itemCode;
        ((IEquippable)item).Equip();
      }
      else throw new Exception($"this item({itemCode}) cannot be equipped.");
    }
    else throw new Exception($"the item({itemCode}) does not exist in your inventory.");
  }

  public void UnEquipItem(string typeOfEquipment)
  {
    if (Items.Keys.Contains(typeOfEquipment))
    {
      if (Items[typeOfEquipment] != Item.Item.Empty)
      {
        inventory.GainItem(Items[typeOfEquipment], 1);
        Items[typeOfEquipment] = Item.Item.Empty;
        ((IEquippable) Item.Item.Get(Items[typeOfEquipment])).UnEquip();
      }
      else throw new Exception($"there are no equipped items: {typeOfEquipment}");
    }
    else throw new Exception($"type {typeOfEquipment} does not exist.");
  }
}