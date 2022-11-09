using System;

namespace GogumaV2.Engine.Skill;

[Serializable]
public sealed class Buff
{
  public uint MaxHp { get; set; }
  
  public uint Hp { get; set; }
  
  public uint MaxMoisture { get; set; }
  
  public uint Moisture { get; set; }
  
  public ushort Armor { get; set; }
  
  public byte CritPercent { get; set; }
  
  public byte CritDamage { get; set; }
  
  public double ReceiveExpGrowthRate { get; set; }
  
  public ushort ArmorPenetration { get; set; }
  
  public byte AvoidanceRate { get; set; }
  
}