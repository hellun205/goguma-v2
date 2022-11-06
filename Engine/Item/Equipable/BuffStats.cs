using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public struct BuffStats
{
  public int MaxHp { get; set; } = 0;
  public int MaxMoisture { get; set; } = 0;
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

  public void Combine(HatBuffStats buffStats)
  {
    MaxHp += buffStats.MaxHp;
    MaxMoisture += buffStats.MaxMoisture;
    ReceiveExpGrowthRate += buffStats.ReceiveExpGrowthRate;
    CritArmor += buffStats.CritArmor;
  }
  
  public void Combine(TopBotBuffStats buffStats)
  {
    MaxHp += buffStats.MaxHp;
    MaxMoisture += buffStats.MaxMoisture;
    ReceiveExpGrowthRate += buffStats.ReceiveExpGrowthRate;
    Armor += buffStats.Armor;
  }
  
  public void Combine(ShoesBuffStats buffStats)
  {
    MaxHp += buffStats.MaxHp;
    MaxMoisture += buffStats.MaxMoisture;
    ReceiveExpGrowthRate += buffStats.ReceiveExpGrowthRate;
    AvoidanceRate += buffStats.AvoidanceRate;
  }
  
}