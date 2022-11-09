using System;
using System.Collections.Generic;
using System.Linq;
using GogumaV2.Engine.Item;
using static GogumaV2.ConsoleUtil;

namespace GogumaV2.Engine.Player;

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

  public void Open(Action<string> callBack) // temporary
  {
    Dictionary<string, List<string>> dict = new();
    foreach (var group in Items.Keys)
      dict.Add(group, new List<string>() { (Items[group] == Item.Item.Empty ? "없음" :Item.Item.Get(Items[group]).Name) });

    Select2d("장비", dict, true, () =>
    {
      if (Selection2d != null)
        callBack(Items[Items.Keys.ToList()[Selection2d.Value.X]]);
      else callBack(null);
    });
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