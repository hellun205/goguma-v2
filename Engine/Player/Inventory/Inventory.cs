using System;
using System.Collections.Generic;
using System.Linq;
using goguma_v2.Engine.Item;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2.Engine.Player.Inventory;

[Serializable]
public class Inventory
{
  public Dictionary<string, List<Item.Item>> Items { get; private set; }
  public Item.Item? SelectedItem { get; private set; }

  public Inventory(string[] groupNames)
  {
    Items = new Dictionary<string, List<Item.Item>>();

    foreach (string name in groupNames)
      Items.Add(name, new List<Item.Item>());
  }

  public void Open(Action callBack)
  {
    Dictionary<string, List<string>> dict = new();
    foreach (var group in Items.Keys)
      dict.Add(group, Items[group].Select(x => x.Name).ToList());

    Select2d("인벤토리", dict, true, () =>
    {
      if (Selection2d != null)
      {
        string Group = Items.Keys.ToList()[Selection2d.Value.X];
        SelectedItem = Items[Group][Selection2d.Value.Y];
      }
      else SelectedItem = null;

      callBack();
    });
  }

  public void GainItem(Item.Item item)
  {
    string type = CheckType(item);
    Items[type].Add(item);
  }

  public void LoseItem(Item.Item item)
  {
    string type = CheckType(item);
    if (Items[type].Contains(item))
    {
      Items[type].Remove(item);
    }
  }

  private string CheckType(Item.Item item)
  {
    string type;
    if (item is IEquippable)
      type = ItemType.EquipableItem;
    else if (item is IConsumable)
      type = ItemType.ConsumableItem;
    else
      type = ItemType.OtherItem;
    
    if (!Items.ContainsKey(type)) throw new Exception($"인벤토리 그룹 중 \"{type}\"(이)가 없습니다.");
    return type;
  }
}