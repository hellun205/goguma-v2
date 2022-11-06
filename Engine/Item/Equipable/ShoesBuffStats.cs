using System;

namespace goguma_v2.Engine.Item.Equipable;

[Serializable]
public struct ShoesBuffStats
{
  public int MaxHp { get; set; } = 0;
  public int MaxMoisture { get; set; } = 0;
  public double ReceiveExpGrowthRate { get; set; } = 0;
  public short AvoidanceRate { get; set; } = 0;

  public ShoesBuffStats()
  {
    
  }
}