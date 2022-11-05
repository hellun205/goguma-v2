using System;
using System.Collections.Generic;
using System.Linq;
using goguma_v2.Engine.Item;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2.Engine.Player.Inventory;

[Serializable]
public class Inventory
{
  public Dictionary<string, List<Item.Item>> Items { get; set; }
  public ItemInfo? SelectedItem { get; private set; }

  public Inventory(string[] names)
  {
    Items = new Dictionary<string, List<Item.Item>>();

    foreach (string name in names)
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
        Item.Item Item = Items[Group][Selection2d.Value.Y];
        SelectedItem = new ItemInfo(Group, Item);
      }
      else SelectedItem = null;

      callBack();
    });
  }
}