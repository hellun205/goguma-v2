using System;

namespace goguma_v2.Engine.Item.Consumable;

[Serializable]
public struct PotionBuffStats
{
  public int Hp { get; set; } = 0;
  public int Moisture { get; set; } = 0;
  public uint Exp { get; set; } = 0;
  
  public PotionBuffStats()
  {
    
  }
}