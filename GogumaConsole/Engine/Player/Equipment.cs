using GogumaConsole.Goguma;
using GogumaConsole.Engine.Item;
using static GogumaConsole.Console.ConsoleUtil;

namespace GogumaConsole.Engine.Player;

[Serializable]
public sealed class Equipment
{
  /// <summary>
  /// Key: Type of Equipment, Value: Code of Item 
  /// </summary>
  public Dictionary<string, string> Items { get; private set; }

  private Inventory inventory;

  public Equipment(IEnumerable<string> typeOfEquipment, Inventory inventory)
  {
    Items = new Dictionary<string, string>();
    this.inventory = inventory;

    foreach (string type in typeOfEquipment)
      Items.Add(type, Manager.Empty);
  }

  public string Open() // temporary
  {
    Dictionary<string, List<string>> dict = Items.Keys.ToDictionary(group => group,
      group => new List<string>() {(Items[group] == Manager.Empty ? "없음" : Items[group].GetItem().Name)});

    var selection = Select2d("장비", dict, "취소");
    return (selection != null ? Items[Items.Keys.ToList()[selection.Value.X]] : null) ?? string.Empty;
  }

  public void EquipItem(string itemCode)
  {
    var item = itemCode.GetItem();
    if (inventory.CheckItem(itemCode))
    {
      if (item is IEquippable equipmentItem)
      {
        string type = equipmentItem.EquipmentType;
        if (Items[type] != Manager.Empty)
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
      if (Items[typeOfEquipment] != Manager.Empty)
      {
        inventory.GainItem(Items[typeOfEquipment], 1);
        Items[typeOfEquipment] = Manager.Empty;
        ((IEquippable) Items[typeOfEquipment].GetItem()).UnEquip();
      }
      else throw new Exception($"there are no equipped items: {typeOfEquipment}");
    }
    else throw new Exception($"type {typeOfEquipment} does not exist.");
  }
}