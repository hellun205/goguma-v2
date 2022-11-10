using System;

namespace GogumaV2.Engine.Skill;

[Serializable]
public sealed class BasicAttack : Skill, IAttackable
{
  public BasicAttack() : base("skill:basic_attack")
  {
    Name = "기본 공격";
    Damage = new Damage()
    {
      AttackDamage = 5
    };
  }

  public Damage Damage { get; set; }
}