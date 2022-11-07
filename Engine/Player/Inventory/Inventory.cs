using System;
using System.Collections.Generic;
using System.Linq;
using goguma_v2.Engine.Item;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2.Engine.Player.Inventory;

[Serializable]
public class Inventory
{
  public Dictionary<string, List<ItemBundle>> Items { get; private set; }
  public Pair<string, int>? SelectedItem { get; private set; }

  public Inventory(string[] groupNames)
  {
    Items = new Dictionary<string, List<ItemBundle>>();

    foreach (string name in groupNames)
      Items.Add(name, new List<ItemBundle>());
  }

  public void Open(Action callBack)
  {
    Dictionary<string, List<string>> dict = new();
    foreach (var group in Items.Keys)
      dict.Add(group, Items[group].Select(x => Item.Item.Get(x.Item).Name).ToList());

    Select2d("인벤토리", dict, true, () =>
    {
      if (Selection2d != null)
        SelectedItem = new Pair<string, int>(Items.Keys.ToList()[Selection2d.Value.X], Selection2d.Value.Y);
      else SelectedItem = null;

      callBack();
    });
  }

  public void GainItem(string itemCode, uint count = 1)
  {
    string type = CheckType(itemCode);
    var max = byte.MaxValue;
    var items = (from item in Items[CheckType(itemCode)]
      where item.Item == itemCode
      orderby item.Count descending 
      select item).ToList();

    if (items.Count != 0)
    {
      foreach (var item in items)
      {
        if (item.Count == max) continue;
        if (item.Count + count > max)
        {
          uint lostCount = item.Count + count - max;
          item.Set(max);
          GainItem(itemCode, lostCount);
          break;
        }
        else
        {
          item.Add((byte)count);
          break;
        }
      }
    }
    else
    {
      if (count > max)
      {
        uint lostCount = count - max;
        Items[type].Add(new ItemBundle(itemCode, max));
        GainItem(itemCode, lostCount);
      }
      else
      {
        Items[type].Add(new ItemBundle(itemCode, (byte)count));
      }
    }
  }

  public void LoseItem(string itemCode, byte count = 1)
  {
    string type = CheckType(itemCode);
    if (Items[type].Contains(item))
    {
      Items[type].Remove(item);
    }
  }

  private string CheckType(string itemCode)
  {
    Item.Item item = Item.Item.Get(itemCode);

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