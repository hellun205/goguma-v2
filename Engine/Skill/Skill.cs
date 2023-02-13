using System;

namespace Goguma.Engine.Skill;

[Serializable]
public abstract class Skill : IEquatable<Skill>, IManageable
{
  public delegate void _OnUse(object sender);

  public string Type => Manager.Types.Skill;
  
  public string Code { get; set; } // format = "[test|game_name]:[skill_name]"
  
  public string Name { get; set; }

  public event _OnUse OnUse;

  public void Use()
  {
    OnUse?.Invoke(this);
  }

  protected Skill(string code)
  {
    this.Init(code);
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