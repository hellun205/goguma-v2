using System;
using System.Windows.Media;

namespace GogumaV2.Engine.Item;

[Serializable]
public abstract class Item : IEquatable<Item>, ISellable, IPurchasable, IManageable
{
  public string Code { get; init; } // format = "[test|game_name]:[item_name]"
  
  public string Name { get; set; } = "none";
  
  public string Description { get; set; } = string.Empty;

  public uint PriceOfSell { get; set; }
  
  public uint PriceOfPurchase { get; set; }
  
  public bool Equals(Item? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Code == other.Code;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((Item) obj);
  }

  public override int GetHashCode() => Code.GetHashCode();

  public override string ToString() => Code;

  protected Item(string code)
  {
    Code = code;
  }

  public event IPurchasable._OnPurchase? OnPurchase;
  
  public void Purchase()
  {
    OnPurchase.Invoke(this, PriceOfPurchase);
  }

  public event ISellable._OnSell? OnSell;
  
  public void Sell()
  {
    OnPurchase.Invoke(this, PriceOfSell);
  }
}