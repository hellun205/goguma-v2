using System;

namespace GogumaWPF.Engine.Item;

[Serializable]
public struct BuffStats
{
  public int MaxHp { get; set; } = 0;
  public int MaxMoisture { get; set; } = 0;
  public int Hp { get; set; } = 0;
  public int Moisture { get; set; } = 0;
  public uint Exp { get; set; } = 0;
  public short AvoidanceRate { get; set; } = 0;
  public double ReceiveExpGrowthRate { get; set; } = 0;
  public short Armor { get; set; } = 0;
  public short CritArmor { get; set; } = 0;
  public short CritPer { get; set; } = 0;
  public short CritDmg { get; set; } = 0;
  public short ArmorPenetration { get; set; } = 0;

  public BuffStats()
  {
    
  }

  public void Addition(BuffStats buffStats)
  {
    MaxHp += buffStats.MaxHp;
    MaxMoisture += buffStats.MaxMoisture;
    Hp += buffStats.Hp;
    Moisture += buffStats.Moisture;
    Exp += buffStats.Exp;
    AvoidanceRate += buffStats.AvoidanceRate;
    ReceiveExpGrowthRate += buffStats.ReceiveExpGrowthRate;
    Armor += buffStats.Armor;
    CritArmor += buffStats.CritArmor;
    CritPer += buffStats.CritPer;
    CritDmg += buffStats.CritDmg;
    ArmorPenetration += buffStats.ArmorPenetration;
  }

  public void Subtract(BuffStats buffStats)
  {
    MaxHp -= buffStats.MaxHp;
    MaxMoisture -= buffStats.MaxMoisture;
    Hp -= buffStats.Hp;
    Moisture -= buffStats.Moisture;
    Exp -= buffStats.Exp;
    AvoidanceRate -= buffStats.AvoidanceRate;
    ReceiveExpGrowthRate -= buffStats.ReceiveExpGrowthRate;
    Armor -= buffStats.Armor;
    CritArmor -= buffStats.CritArmor;
    CritPer -= buffStats.CritPer;
    CritDmg -= buffStats.CritDmg;
    ArmorPenetration -= buffStats.ArmorPenetration;
  }
}