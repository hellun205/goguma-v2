using System;

namespace GogumaV2.Engine.Skill;

[Serializable]
public abstract class Skill : IEquatable<Skill>
{
  public delegate void _OnUse(object sender); 
  
  public string Code { get; init; }
  
  public string Name { get; set; }

  public event _OnUse OnUse;

  public void Use()
  {
    OnUse?.Invoke(this);
  }

  protected Skill(string code)
  {
    Code = code;
  }

  public bool Equals(Skill? other)
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
    return Equals((Skill) obj);
  }

  public override int GetHashCode()
  {
    return Code.GetHashCode();
  }
}