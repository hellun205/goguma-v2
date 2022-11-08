using System;
using System.Collections.Generic;
using System.Linq;
using goguma_v2.Engine.Item;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2.Engine.Player;

[Serializable]
public sealed class Inventory
{
  public Dictionary<string, List<ItemBundle>> Items { get; private set; }

  public Inventory(string[] groupNames)
  {
    Items = new Dictionary<string, List<ItemBundle>>();

    foreach (string name in groupNames)
      Items.Add(name, new List<ItemBundle>());
  }

  public void Open(Action<Pair<string,int>?> callBack)
  {
    Dictionary<string, List<string>> dict = new();
    foreach (var group in Items.Keys)
      dict.Add(group, Items[group].Select(x => x.ToString()).ToList());

    Select2d("인벤토리", dict, true, () =>
    {
      if (Selection2d != null)
        callBack(new Pair<string, int>(Items.Keys.ToList()[Selection2d.Value.X], Selection2d.Value.Y));
      else callBack(null);
    });
  }

  public void GainItem(string itemCode, uint count = 1)
  {
    string type = CheckType(itemCode);
    var max = byte.MaxValue;
    var items = (from item in Items[type]
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
          return;
        }
        else
        {
          item.Add((byte) count);
          return;
        }
      }

      if (count > max)
      {
        Items[type].Add(new ItemBundle(itemCode, max));
        GainItem(itemCode, count - max);
      }
      else
      {
        Items[type].Add(new ItemBundle(itemCode, (byte)count));
      }
    }
    else
    {
      if (count > max)
      {
        Items[type].Add(new ItemBundle(itemCode, max));
        GainItem(itemCode, count - max);
      }
      else
      {
        Items[type].Add(new ItemBundle(itemCode, (byte) count));
      }
    }
  }

  public void LoseItem(string itemCode, uint count = 1)
  {
    string type = CheckType(itemCode);
    var max = byte.MaxValue;
    var items = (from item in Items[type]
      where item.Item == itemCode
      orderby item.Count
      select item).ToList();

    if (items.Count != 0)
    {
      if (count > max)
      {
        foreach (var item in items)
        {
          if (item.Count == max)
          {
            Items[type].Remove(item);
            LoseItem(itemCode, count - max);
            break;
          }
          else
          {
            uint lostCount = count - item.Count;
            Items[type].Remove(item);
            LoseItem(itemCode, lostCount);
            break;
          }
        }
      }
      else
      {
        foreach (var item in items)
        {
          if (item.Count < count)
          {
            var lostCount = count - item.Count;
            Items[type].Remove(item);
            LoseItem(itemCode, lostCount);
            break;
          }
          else if (item.Count > count)
          {
            item.Remove((byte)count);
            break;
          }
          else if (item.Count == count)
          {
            Items[type].Remove(item);
            break;
          }
        }
      }
    }
  }

  public void Clear()
  {
    foreach (var type in Items.Keys)
      Items[type].Clear();
  }
  
  public void Clear(string type)
  {
    Items[type].Clear();
  }
  
  public void LoseAllItem(string itemCode)
  {
    string type = CheckType(itemCode);
    var items = (from item in Items[type]
      where item.Item == itemCode
      select item).ToList();

    foreach (var item in items)
      Items[type].Remove(item);
  }

  public string CheckType(string itemCode)
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

  public bool CheckItem(string itemCode, uint count = 1) => Items[CheckType(itemCode)].Exists(x => x.Item == itemCode && x.Count >= count);
}