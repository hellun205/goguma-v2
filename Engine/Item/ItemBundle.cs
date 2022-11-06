using System;

namespace goguma_v2.Engine.Item;

public struct ItemBundle : IEquatable<ItemBundle>
{
  public string Item { get; private set; }
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
  
  public bool Equals(ItemBundle other)
  {
    return Item == other.Item;
  }

  public override bool Equals(object? obj)
  {
    return obj is ItemBundle other && Equals(other);
  }

  public override int GetHashCode()
  {
    return Item.GetHashCode();
  }
}