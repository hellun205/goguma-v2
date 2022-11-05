namespace goguma_v2.Engine.Item;

public struct ItemInfo
{
  public string Group { get; set; }
  public Item Item { get; set; }

  public ItemInfo(string group, Item item)
  {
    Group = group;
    Item = item;
  }

  public override string ToString() => $"[{Group}] {Item.Name}";
}