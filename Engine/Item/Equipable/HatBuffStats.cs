using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public struct HatBuffStats
{
  public int MaxHp { get; set; } = 0;
  public int MaxMoisture { get; set; } = 0;
  public double ReceiveExpGrowthRate { get; set; } = 0;
  public short CritArmor { get; set; } = 0;
  
  public HatBuffStats()
  {
    
  }
}