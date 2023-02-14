using System;
using System.Collections.Generic;
using System.Linq;
using Goguma.Engine.Item;
using Goguma.Game;
using Goguma.Screen;
using static Goguma.Game.Main;

namespace Goguma.Engine.Player;

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
      Items.Add(type, EmptyCode);
  }

  public void Open(Action<string> callBack) // temporary
  {
    Dictionary<string, List<string>> dict = Items.Keys.ToDictionary(group => group, group => new List<string>() {(Items[group] == EmptyCode ? "없음" : Items[group].GetItem().Name)});

    Main.screen.Select2d(dict, "취소", selection =>
    {
      callBack((selection != null ? Items[Items.Keys.ToList()[selection.Value.X]] : null) ?? string.Empty);
    });
  }

  public void EquipItem(string itemCode)
  {
    var item = itemCode.GetItem();
    if (inventory.CheckItem(itemCode))
    {
      if (item is IEquippable equipmentItem)
      {
        string type = equipmentItem.EquipmentType;
        if (Items[type] != EmptyCode)
          UnEquipItem(type);
        inventory.LoseItem(itemCode, 1);
        Items[type] = itemCode;
        equipmentItem.Equip();
      }
      else throw new Exception($"this item({itemCode}) cannot be equipped.");
    }
    else throw new Exception($"the item({itemCode}) does not exist in your inventory.");
  }

  public void UnEquipItem(string typeOfEquipment)
  {
    if (Items.ContainsKey(typeOfEquipment))
    {
      if (Items[typeOfEquipment] != EmptyCode)
      {
        inventory.GainItem(Items[typeOfEquipment], 1);
        Items[typeOfEquipment] = EmptyCode;
        ((IEquippable) Items[typeOfEquipment].GetItem()).UnEquip();
      }
      else throw new Exception($"there are no equipped items: {typeOfEquipment}");
    }
    else throw new Exception($"type {typeOfEquipment} does not exist.");
  }
}