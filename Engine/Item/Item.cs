using System;
using System.Windows.Media;

namespace goguma_v2.Engine.Item;

[Serializable]
public abstract partial class Item : IEquatable<Item>
{
  public string Code { get; init; }
  public string Name { get; set; } = "none";
  public abstract string Type { get; }
  public string Description { get; set; } = string.Empty;
  public Brush DefaultColor { get; set; } = ConsoleUtil.MainScreen.FGColor;
  public string Display => $"[ {Type} ] {Name}";

  public bool Equals(Item? other)
  {
    if (ReferenceEquals(null, other)) return false;
    return Code == other.Code;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((Item) obj);
  }

  public override int GetHashCode() => Code.GetHashCode();

  public override string ToString() => Code;

  protected Item(string code)
  {
    Code = code;
  }
  
  public abstract void OnUse();
}