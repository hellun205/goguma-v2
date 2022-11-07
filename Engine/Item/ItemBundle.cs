using System;

namespace goguma_v2.Engine.Item;

public sealed class ItemBundle : IEquatable<ItemBundle>
{
  public string Item { get; init; }
  public Byte Count { get; private set; }

  public ItemBundle(string itemCode)
  {
    Item = itemCode;
    Count = 0;
  }

  public ItemBundle(string itemCode, Byte count) : this(itemCode)
  {
    Count = count;
  }

  public void Add(Byte amount = 1)
  {
    Count = (Byte)Math.Min(Byte.MaxValue, Count + amount);
  }

  public void Remove(Byte amount = 1)
  {
    Count = (Byte) Math.Max(Byte.MinValue, Count - amount);
  }

  public void Set(Byte value)
  {
    Count = value;
  }

  public bool Equals(ItemBundle? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Item == other.Item && Count == other.Count;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((ItemBundle) obj);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Item, Count);
  }

  public override string ToString() => $"{Engine.Item.Item.Get(Item).Name}{(Count == 1 ? "" : $" {Count}개")}";
}