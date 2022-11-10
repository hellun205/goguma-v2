using System;

namespace GogumaV2.Engine.Skill;

[Serializable]
public sealed class Damage
{
  public uint AttackDamage { get; set; }
  
  public uint MoisturePower { get; set; }
  
  public byte CritPercent { get; set; }
  
  public byte CritDamage { get; set; }
  
  public ushort ArmorPenetration { get; set; }
}