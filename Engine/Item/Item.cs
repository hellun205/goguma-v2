using System;
using System.Windows.Media;
using Goguma.Game;

namespace Goguma.Engine.Item;

[Serializable]
public abstract class Item : IEquatable<Item>, ITradable, IGameObject
{
  public string Type => GameObjectManager.Types.Item;
  
  public string Code { get; set; } // format = "[test|game_name]:[item_name]"
  
  public string Name { get; set; } = "none";
  
  public string Description { get; set; } = string.Empty;

  public uint PriceOfSell { get; set; }
  
  public event ITradable._OnPurchase? OnPurchase;
  
  public event ITradable._OnSell? OnSell;

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
    this.Init(code);
  }

  public void Purchase()
  {
    OnPurchase.Invoke(this, PriceOfPurchase);
  }
  
  public void Sell()
  {
    OnPurchase.Invoke(this, PriceOfSell);
  }
  
  public string CheckType()
  {
    string type = this switch
    {
      IEquippable => ItemType.EquipableItem,
      IConsumable => ItemType.ConsumableItem,
      _ => ItemType.OtherItem
    };

    return type;
  }
}